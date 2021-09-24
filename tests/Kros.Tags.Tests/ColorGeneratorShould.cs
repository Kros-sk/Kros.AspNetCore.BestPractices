using FluentAssertions;
using Kros.Tags.Api.Application.Services;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Kros.Tags.Tests
{
    public class ColorGeneratorShould
    {
        private readonly ITableStorageManagementService _tableStorageManagementService =
            Substitute.For<ITableStorageManagementService>();

        [Fact]
        public void ReturnRandomARGBColor()
        {
            var organizationId = 1;
            var colorValue = 0;
            var oldColorValue = 0;

            var usedColor1 = -48521;
            var usedColor2 = -8521632;
            var existingColors = createExistingColors(usedColor1, usedColor2, organizationId);

            ColorManagementService colorManagementService = CreateColorManagementService(existingColors, organizationId);

            var generatedColor = colorManagementService.CheckAndGenerateColor(organizationId, colorValue, oldColorValue);

            generatedColor.Should().NotBe(0);
        }

        [Theory]
        [InlineData(-123, -45556)]
        [InlineData(-5215748, -555555)]
        public void NotReturnUsedColor(int usedColor1, int usedColor2)
        {
            var organizationId = 1;
            var colorValue = 0;
            var oldColorValue = 0;

            var existingColors = createExistingColors(usedColor1, usedColor2, organizationId);

            ColorManagementService colorManagementService = CreateColorManagementService(existingColors, organizationId);

            var generatedColor = colorManagementService.CheckAndGenerateColor(organizationId, colorValue, oldColorValue);

            generatedColor.Should().NotBe(usedColor1);
            generatedColor.Should().NotBe(usedColor2);
        }

        [Fact]
        public void HaveAvailableColorWhichIsNotLongerUsed()
        {
            var organizationId = 1;
            var colorValue = -48521;
            var oldColorValue = 0;

            var usedColor1 = -48521;
            var usedColor2 = -8521632;
            var existingColors = createExistingColors(usedColor1, usedColor2, organizationId);

            ColorManagementService colorManagementService = CreateColorManagementService(existingColors, organizationId);

            var generatedColor = colorManagementService.CheckAndGenerateColor(organizationId, colorValue, oldColorValue);

            generatedColor.Should().Be(0);

            existingColors.RemoveAt(0);

            generatedColor = colorManagementService.CheckAndGenerateColor(organizationId, colorValue, oldColorValue);

            generatedColor.Should().NotBe(0);
        }

        [Theory]
        [InlineData(-95123, -6584)]
        [InlineData(-195222, -4877)]
        public void ReturnUsedColors(int usedColor1, int usedColor2)
        {
            var organizationId = 1;

            var existingColors = createExistingColors(usedColor1, usedColor2, organizationId);

            ColorManagementService colorManagementService = CreateColorManagementService(existingColors, organizationId);

            var usedColors = colorManagementService.GetUsedColors(organizationId);

            usedColors.ElementAt(0).ColorValue.Should().Be(usedColor1.ToString());
            usedColors.ElementAt(1).ColorValue.Should().Be(usedColor2.ToString());
        }

        [Fact]
        public void AllowUseSameColorInDifferentOrganization()
        {
            var organizationId1 = 1;
            var organizationId2 = 2;

            var usedColor1 = -48521;
            var usedColor2 = -8521632;

            var existingColors = createExistingColors(usedColor1, usedColor2, organizationId1);

            _tableStorageManagementService.GetAllValuesForPartition(organizationId2)
                .Returns(new List<ColorEntity>().AsEnumerable());

            ColorManagementService colorManagementService = CreateColorManagementService(existingColors, organizationId1);

            var generatedColorOrganization1 =
                colorManagementService.CheckAndGenerateColor(organizationId1, usedColor1, 0);
            var generatedColorOrganization2 =
                colorManagementService.CheckAndGenerateColor(organizationId2, usedColor1, 0);

            generatedColorOrganization1.Should().Be(0);
            generatedColorOrganization2.Should().NotBe(0);
        }

        private ColorManagementService CreateColorManagementService(List<ColorEntity> existingColors, int organizationId)
        {
            _tableStorageManagementService.GetAllValuesForPartition(organizationId).Returns(existingColors.AsEnumerable());
            ColorManagementService colorManagementService = new ColorManagementService(_tableStorageManagementService);
            return colorManagementService;
        }

        private List<ColorEntity> createExistingColors(int usedColor1, int usedColor2, int organizationId)
        {
            var colorEntity1 = new ColorEntity { ColorValue = usedColor1.ToString(), OrganizationId = organizationId.ToString() };
            var colorEntity2 = new ColorEntity { ColorValue = usedColor2.ToString(), OrganizationId = organizationId.ToString() };

            List<ColorEntity> colorsList = new List<ColorEntity>();
            colorsList.Add(colorEntity1);
            colorsList.Add(colorEntity2);

            return colorsList;
        }
    }
}
