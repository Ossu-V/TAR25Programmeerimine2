using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.RealEstateTest.Macros;
using ShopTARpe25.RealEstateTest.Mock;

namespace ShopTARpe25.RealEstateTest
{
    // Ühine baasklass testidele, et ei peaks igas failis uuesti andmebaasi ja teenuseid seadistama
    // abstract - seda klassi otse ei käivitata, teised testiklassid lihtsalt pärivad selle sisu
    public abstract class TestBase
    {
        // Hoiab endas valmis ehitatud tööriistakasti (DI container), kust testid vajalikke teenuseid võtavad
        // IServiceProvider - süsteem, mille kaudu saab registreeritud teenuseid koodis kätte
        // protected - nähtav ainult selles klassis ja sellest pärivates testiklassides
        protected IServiceProvider ServiceProvider { get; set; }

        // Constructor, mis käivitub automaatselt enne iga testi tegemist
        protected TestBase()
        {
            // Loob tühja nimekirja teenustest, mida testide tegemiseks vaja läheb
            // ServiceCollection - nimekiri, kuhu kogutakse kokku kõik vajaminevad service'id
            var services = new ServiceCollection();

            // Kutsub välja SetupServices meetodi, mis täidab nimekirja teenuste ja andmebaasiga
            SetupServices(services);

            // Kutsub välja BuildServiceProvider meetodi, mis ehitab nimekirjast töökorras DI container'i
            // BuildServiceProvider() - teeb teenuste nimekirjast reaalse tööriistakasti
            ServiceProvider = services.BuildServiceProvider();
        }

        // Meetod, kus pannakse paika kõik testideks vajalikud service'id ja andmebaas
        // virtual - lubab teistel testiklassidel seda seadistust vajadusel muuta (override)
        public virtual void SetupServices(IServiceCollection services)
        {
            // Ütleb testile, et kui küsitakse IRealEstateServices interface'i, tuleb anda RealEstateServices klass
            // AddScoped - loob teenusest ühe uue koopia (instance'i) iga testi jaoks, et testid üksteist ei segaks
            services.AddScoped<IRealEstateServices, RealEstateServices>();
            //sellepärast, et see oli seotud failide lisemisega ja failide lisamine omakorda
            //oli seoses IHostEnvironmentiga
            services.AddScoped<IFileServices, FileServices>();
            services.AddScoped<IHostEnvironment, MockIHostEnvironment>();

            // Seadistab testide jaoks andmebaasi
            // AddDbContext - lisab andmebaasiühenduse (ShopTARpe25Context) teenuste hulka
            services.AddDbContext<ShopTARpe25Context>(x =>
            {
                // Kasutab päris SQL-andmebaasi asemel ajutist mälupõhist andmebaasi (RAM-is)
                // UseInMemoryDatabase - loob kiire liba-andmebaasi arvuti mällu, et päris andmed ei muutuks
                x.UseInMemoryDatabase("TEST");

                // Peidab konsoolist hoiatused, kuna mälubaas ei toeta tehingute tagasivõtmist (transaction'eid)
                // ConfigureWarnings / Ignore - käsk ignoreerida seda konkreetset hoiatust
                x.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            // Kutsub välja RegisterMacros meetodi, mis registreerib testandmete loomise abiklassid (macro'd)
            RegisterMacros(services);
        }

        // Meetod ressursside vabastamiseks pärast testi (hetkel tühi)
        // Dispose - kasutatakse pärast testi lõppu ajutiste andmete kustutamiseks mälust
        public void Dispose()
        {

        }

        // Lühike abimeetod teenuse kättesaamiseks testis (et ei peaks kirjutama pikka koodirida)
        // <T> (Generic) - tähendab ükskõik millist tüüpi service'it, mida sa parajasti küsid
        protected T Svc<T>()
        {
            // Kutsub välja GetService meetodi, mis otsib DI container'ist üles ja tagastab küsitud teenuse
            return ServiceProvider.GetService<T>();
        }

        // Otsib projektist automaatselt üles kõik macro'd ja lisab need teenuste hulka
        // private - seda meetodit saab kasutada ainult siin TestBase klassi sees
        private void RegisterMacros(IServiceCollection services)
        {
            // Võtab aluseks IMacros interface'i, et teada, milliseid klasse otsida
            // typeof - küsib IMacros interface'i tüübiinfo, et seda saaks koodis uurida
            var macroBaseType = typeof(IMacros);

            // Otsib projektist üles kõik tavalised klassid, mis kasutavad IMacros interface'i
            // Assembly.GetTypes() - vaatab läbi kogu projekti faili ja võtab sealt kõik klassid
            // IsAssignableFrom - kontrollib, kas leitud klass kasutab IMacros interface'i
            var macros = macroBaseType.Assembly.GetTypes()
                .Where(t => macroBaseType.IsAssignableFrom(t)
                && !t.IsInterface && !t.IsAbstract);

            // Lisab kõik leitud macro'd teenuste hulka
            // AddSingleton - loob igast macro'st mällu ainult ühe koopia (instance'i) ja kasutab seda kõigis testides
            foreach (var macro in macros)
            {
                services.AddSingleton(macro);
            }
        }
    }
}
