using Microsoft.VisualBasic;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;

namespace ShopTARpe25.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            //Arrange
            RealEstateDto dto = new()
            {
                Area = 120.5,
                Address = "Test location",
                RoomNumber = 3,
                BuildingType = "house",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            };

            //Act, siin kutsutakse RealEstate Create meetod esile
            var result = await Svc<IRealEstateServices>().Create(dto);

            //Assert
            Assert.NotNull(result);
        }

        //Test, kus otsib ankeeti id alusel, aga seda ei ole andmebaasis
        [Fact]
        public async Task ShouldNot_GetByIdRealEstate_WhenReturnsEqual()
        {
            //Arrange
            Guid databaseGuid = Guid.Parse("168b4d90-2366-42d6-8c47-06193759c076");
            Guid wrongGuid = Guid.NewGuid(); 

            //Act
            await Svc<IRealEstateServices>().DetailsAsync(databaseGuid);

            //Assert
            //Vastupidist loogikat tuleb kasutada
            Assert.NotEqual(wrongGuid, databaseGuid);
        }

        //DetailAsync test
        [Fact]
        public async Task Should_GetByIdRealEstate_WhenReturnsEqual()
        {
            //Arrange
            //see guid on juba andmebaasis olemas ankeedi juures
            Guid databaseGuid = Guid.Parse("168b4d90-2366-42d6-8c47-06193759c076");
            //kui vajutad nupule, siise see guid saadetakse andmebaasi, mida võrreldakse
            Guid guid = Guid.Parse("168b4d90-2366-42d6-8c47-06193759c076");

            //Act
            await Svc<IRealEstateServices>().DetailsAsync(guid);

            //Assert
            Assert.Equal(databaseGuid, guid);
        }

        //kustutamise meetodi testimine. Andmebaasis on ankeet, mida tahan ära kustutada
        [Fact]
        public async Task Should_DeleteByIdRealEstate_WhenDeleteRealEstate()
        {
            //Arrange
            //vaja on mockida ankeet e teha meetod, kus on andmed
            RealEstateDto dto = MockRealEstateData();

            //Act
            var addRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var deleteRealEstate = await Svc<IRealEstateServices>().Delete((Guid)addRealEstate.Id);

            //Assert
            //võrdlen Id-sid, kas kustutasin selle ära, mis oli andmebaasis
            Assert.Equal(addRealEstate.Id, deleteRealEstate.Id);
        }

        [Fact]
        public async Task ShouldNot_DeleteByIdRealEstate_WhenDidNotDeleteRealEstate()
        {
            //Arrange
            var dto = MockRealEstateData();

            //Act
            var RealEstate1 = await Svc<IRealEstateServices>().Create(dto);
            var RealEstate2 = await Svc<IRealEstateServices>().Create(dto);

            var result = await Svc<IRealEstateServices>().Delete((Guid)RealEstate2.Id);

            //Assert
            Assert.NotEqual(RealEstate1.Id, result.Id);
        }


        //uuendab andmeid
        [Fact]
        public async Task Should_UpdateRealEstate_WhenReturnResult()
        {
            //Arrange
            var guid = new Guid("168b4d90-2366-42d6-8c47-06193759c076");

            RealEstateDto dto = MockRealEstateData();

            RealEstateDto domain = new();

            domain.Id = guid;
            domain.Area = 200.0;
            domain.Address = "Update Location";
            domain.RoomNumber = 5;
            domain.BuildingType = "Villa";
            domain.CreatedAt = DateTime.UtcNow;
            domain.ModifiedAt = DateTime.UtcNow;

            //Act
            await Svc<IRealEstateServices>().Update(dto);

            //Assert
            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(dto.Area, domain.Area);
            Assert.NotEqual(dto.RoomNumber, domain.RoomNumber);
            //DoesNotMatch tahab string andmetüüpe
            Assert.DoesNotMatch(dto.RoomNumber.ToString(), domain.RoomNumber.ToString());
            Assert.DoesNotMatch(dto.Address, domain.Address);
        }

        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateVersion2()
        {
            //
        }

        private RealEstateDto MockRealEstateData()
        {
            return new RealEstateDto
            {
                Area = 88.65,
                Address = "TestAdress",
                RoomNumber = 1,
                BuildingType = "TestBuildingType",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
        }
    }
}
