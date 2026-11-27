

using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;

namespace ShopTARpe25.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            // Arrange
            RealEstateDto dto = new()
            {
                Area = 120.5,
                Address = "Test location",
                RoomNumber = 3,
                BuildingType = "house",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            };

            //Act Siin tuleb kutsuda RealEstate Create meetod esile
            var result = await Svc<IRealEstateServices>().Create(dto);

            //Assert
            Assert.NotNull(result);
        }

        //teha test, kus otsib ankeeti id alusel, aga seda ei ole andmebaasis
        //tuli vaade, mis ei sobi rakenduse jaoks.
        [Fact]
        public async Task ShouldNot_GetByIdRealEstate_WhenReturnsEqual()
        {
            //Arrange
            Guid databaseGuid = Guid.Parse("d3661e93-4701-40a7-9770-ef18cd3a3253");
            Guid wrongGuid = Guid.NewGuid();

            //Act
            await Svc<IRealEstateServices>().DetailsAsync(databaseGuid);

            //Assert
            //vastupidist loogikat tuleb kasutada
            Assert.NotEqual(wrongGuid, databaseGuid);
        }

        [Fact]
        public async Task Should_GetByIdRealestate_WhenReturnsEqual()
        {
            //arrange
            //see guid on juba andmebaasis olemas ankeedi juures
            Guid databaseGuid = Guid.Parse("d3661e93-4701-40a7-9770-ef18cd3a3253");
            //kui vajutad nupule, siis see guid saadetakse andmebaasi, mida võrreldakse
            Guid guid = Guid.Parse("d3661e93-4701-40a7-9770-ef18cd3a3253");

            //act
            await Svc<IRealEstateServices>().DetailsAsync(guid);

            //assert
            Assert.Equal(databaseGuid, guid);
        }

        //kustutamise meetotit on vaja testida. Andmebaasis on ankeet, mida 
        //tahan ära kustutada
        [Fact]
        public async Task Should_DeleteByIdRealEstate_WhenDeleteRealEstate()
        {
            //arrange
            //vaja oleks mockida ankeeti e teha meetod, kus on nakeedi andmed
            RealEstateDto dto = MockRealEstateData();

            //act
            var addRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var deleteRealEstate = await Svc<IRealEstateServices>().Delete((Guid)addRealEstate.Id);

            //assert
            //võrdlen Id-sid, kas kustutasin selle ära, mis oli andmebaasis
            Assert.Equal(addRealEstate.Id, deleteRealEstate.Id);
        }

        //mul on kaks ankeeti loodud ja ühe kustutan ära
        //assertis võrdlen, kas need kaks on ühesugused
        [Fact]
        public async Task ShouldNot_DeleteByIdRealEstate_WhenDidNotDeleteRealestate()
        {
            //arrange
            var dto = MockRealEstateData();

            //act 
            var realEstate1 = await Svc<IRealEstateServices>().Create(dto);
            var realEstate2 = await Svc<IRealEstateServices>().Create(dto);

            var result = await Svc<IRealEstateServices>().Delete((Guid)realEstate2.Id);

            //assert
            Assert.NotEqual(realEstate1.Id, result.Id);
        }

        //uuendab andmeid
        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateData()
        {
            //arrange
            var guid = new Guid("d3661e93-4701-40a7-9770-ef18cd3a3253");

            RealEstateDto dto = MockRealEstateData();

            RealEstateDto domain = new();

            domain.Id = guid;
            domain.Area = 200.0;
            domain.Address = "Update Location";
            domain.RoomNumber = 5;
            domain.BuildingType = "Villa";
            domain.CreatedAt = DateTime.UtcNow;
            domain.ModifiedAt = DateTime.UtcNow;

            //act
            await Svc<IRealEstateServices>().Update(dto);

            //assert, 5tk teha
            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(dto.Area, domain.Area);
            Assert.NotEqual(dto.RoomNumber, domain.RoomNumber);
            //Võrrelda RoomNumbrit ja kasutada DoesNotMatch
            //DoesNotMatch tahab string andmetüüpe
            Assert.DoesNotMatch(dto.RoomNumber.ToString(), domain.RoomNumber.ToString());
            Assert.DoesNotMatch(dto.Address, domain.Address);
        }

        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateDataVersion2()
        {
            //tuleb kasutada MockRealEstateDataUpdate ja MockRealEstateData2
            RealEstateDto dto = MockRealEstateData2();
            RealEstateDto updateDto = MockRealEstateDataUpdate();

            var createRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var result = await Svc<IRealEstateServices>().Update(updateDto);

            //assert
            Assert.DoesNotMatch(createRealEstate.Address, result.Address);
            Assert.NotEqual(createRealEstate.ModifiedAt, result.ModifiedAt);
        }

        //kasutame ankeeti, kus kõik andmed on NULL
        //tuleb kasutada Create ja Update ära
        [Fact]
        public async Task ShouldNot_UpdateRealEstate_WhenDidNotUpdateData()
        {
            //arrange
            //kasutate MockRealEstateData meetodit, kus on andmed
            //tuleb kasutada Create meetodit, et andmed luua
            RealEstateDto dto = MockRealEstateData();
            RealEstateDto nullDto = MockNullRealEstateData();

            //tuleb teha uus meetod nimega MockNullRealEstateData(),
            //kus on tühjad andmed e null või ""
            var createRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var result = await Svc<IRealEstateServices>().Update(nullDto);

            //assert
            //toimub võrdlemine, et andmed ei ole võrdsed
            Assert.NotEqual(createRealEstate.Id, result.Id);
            Assert.NotEqual(createRealEstate.Address, result.Address);
        }


        private RealEstateDto MockRealEstateData()
        {
            return new RealEstateDto
            {
                Area = 88.65,
                Address = "TestAddress",
                RoomNumber = 1,
                BuildingType = "TestBuildingType",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
        }

        private RealEstateDto MockRealEstateData2()
        {
            return new RealEstateDto
            {
                Area = 120.00,
                Address = "Sample Address",
                RoomNumber = 8,
                BuildingType = "House",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
        }

        private RealEstateDto MockRealEstateDataUpdate()
        {
            return new RealEstateDto
            {
                Area = 100.00,
                Address = "Woodland",
                RoomNumber = 2,
                BuildingType = "Hideout",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
        }

        private RealEstateDto MockNullRealEstateData()
        {
            return new RealEstateDto
            {
                Area = null,
                Address = null,
                RoomNumber = 0,
                BuildingType = null,
                CreatedAt = null,
                ModifiedAt = null
            };
        }

    }
}