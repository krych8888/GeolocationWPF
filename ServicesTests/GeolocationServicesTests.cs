using AutoMapper;
using DataService.Repository.Interfaces;
using Entities.DbSet;
using EventBookingApi.MappingProfiles;
using FluentAssertions;
using IpStack.Services;
using Moq;
using Services;

namespace ServicesTests;

public class GeolocationServicesTests
{
    private readonly Mock<IIpStackService> _ipStackService;
    private readonly Mock<IUnitOfWork> _unitOfWork;


    private GeolocationService _sut;


    public GeolocationServicesTests()
    {
        _ipStackService = new();
        var mapper = new Mapper(new MapperConfiguration(m => m.AddProfile(new GeolocationProfile())));
        _unitOfWork = new();

        _sut = new GeolocationService(_ipStackService.Object, mapper, _unitOfWork.Object);
    }

    [InlineData(@"https://stackoverflow.com/questions/5710485/what-does-use-of-unassigned-local-variable-mean")]
    [InlineData("https://ipstack.com/documentation")]
    [InlineData("134.201.250.155")]
    [Theory]
    public async Task GetGeolocationData_correctly_consumes_ip_and_url_site_addresses(string address)
    {
        //Arrange
        _unitOfWork.Setup(x => x.Geolocations.GetByIp(It.IsAny<string>())).ReturnsAsync(new GeolocationData());

        //Act
        var result = await _sut.GetGeolocationData(address);

        //Assert
        _unitOfWork.Verify(x => x.Geolocations.GetByIp(It.IsAny<string>()), Times.Once);
        result.IsDownloaded.Should().Be(true);
    }

    [InlineData("xyz")]
    [InlineData(" ")]
    [InlineData("135a")]
    [Theory]
    public async Task GetGeolocationData_throws_error_when_siteAddress_not_valid(string address)
    {
        //Arrange
        _unitOfWork.Setup(x => x.Geolocations.GetByIp(It.IsAny<string>())).ReturnsAsync(new GeolocationData());

        //Act
        Func<Task> action = async () => await _sut.GetGeolocationData(address);

        //Assert
        _unitOfWork.Verify(x => x.Geolocations.GetByIp(It.IsAny<string>()), Times.Never);
        action.Should().ThrowAsync<UriFormatException>();
    }

    [InlineData("xyz")]
    [InlineData(" ")]
    [InlineData("135a")]
    [Theory]
    public async Task AddGeolocationData_throws_error_when_ipAddress_not_valid(string address)
    {
        //Arrange
        var data = new GeolocationData();
        data.Ip = address;
        _unitOfWork.Setup(x => x.Geolocations.Create(It.IsAny<GeolocationData>())).ReturnsAsync(new GeolocationData());

        //Act
        Func<Task> action = async () => await _sut.AddGeolocationData(data);

        //Assert
        _unitOfWork.Verify(x => x.Geolocations.Create(It.IsAny<GeolocationData>()), Times.Never);
        action.Should().ThrowAsync<UriFormatException>();
    }

    [InlineData("134.201.250.155")]
    [InlineData("184.201.250.155")]
    [InlineData("172.68.159.73")]
    [Theory]
    public async Task AddGeolocationData_correctly_consumes_ip_site_addresse(string address)
    {
        //Arrange
        var data = new GeolocationData();
        data.Ip = address;
        _unitOfWork.Setup(x => x.Geolocations.Create(It.IsAny<GeolocationData>())).ReturnsAsync(new GeolocationData());
        _unitOfWork.Setup(x => x.CompleteAsync()).ReturnsAsync(true);

        //Act
        var result = await _sut.AddGeolocationData(data);

        //Assert
        _unitOfWork.Verify(x => x.Geolocations.Create(It.IsAny<GeolocationData>()), Times.Once);
        result.IsDownloaded.Should().Be(true);
    }

    [InlineData("xyz")]
    [InlineData(" ")]
    [InlineData("135a")]
    [Theory]
    public async Task DeleteGeolocationData_throws_error_when_ipAddress_not_valid(string address)
    {
        //Arrange
        _unitOfWork.Setup(x => x.Geolocations.Delete(It.IsAny<string>())).ReturnsAsync(address);

        //Act
        Func<Task> action = async () => await _sut.DeleteGeolocationData(address);

        //Assert
        _unitOfWork.Verify(x => x.Geolocations.Delete(It.IsAny<string>()), Times.Never);
        action.Should().ThrowAsync<UriFormatException>();
    }

    [InlineData("134.201.250.155")]
    [InlineData("184.201.250.155")]
    [InlineData("172.68.159.73")]
    [Theory]
    public async Task DeleteGeolocationData_correctly_consumes_ip_site_addresse(string address)
    {
        //Arrange
        var data = new GeolocationData();
        data.Ip = address;
        _unitOfWork.Setup(x => x.Geolocations.Delete(It.IsAny<string>())).ReturnsAsync(address);
        _unitOfWork.Setup(x => x.CompleteAsync()).ReturnsAsync(true);

        //Act
        var result = await _sut.DeleteGeolocationData(address);

        //Assert
        _unitOfWork.Verify(x => x.Geolocations.Delete(It.IsAny<string>()), Times.Once);
        result.Should().Be(address);
    }
}