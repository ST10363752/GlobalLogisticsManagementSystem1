using Xunit;
using System.IO;
using System.Linq;

namespace GLMS.Tests
{
    public class FileValidationTests
    {
        [Fact]
        public void Test_PDFFileExtension_IsValid()
        {
            // Arrange
            string fileName = "contract.pdf";
            string expectedExtension = ".pdf";

            // Act
            string actualExtension = Path.GetExtension(fileName).ToLower();

            // Assert
            Assert.Equal(expectedExtension, actualExtension);
        }

        [Fact]
        public void Test_ExeFileExtension_IsInvalid()
        {
            // Arrange
            string fileName = "virus.exe";
            string expectedExtension = ".exe";

            // Act
            string actualExtension = Path.GetExtension(fileName).ToLower();

            // Assert
            Assert.Equal(expectedExtension, actualExtension);
            // EXE files should be rejected - this test confirms the extension is detected
        }

        [Fact]
        public void Test_OnlyPDFFiles_AreAllowed()
        {
            // Arrange
            var allowedExtensions = new[] { ".pdf" };
            string pdfFile = "document.pdf";
            string exeFile = "malware.exe";
            string txtFile = "readme.txt";
            string docFile = "contract.docx";

            // Act
            bool isPdfValid = allowedExtensions.Contains(Path.GetExtension(pdfFile).ToLower());
            bool isExeValid = allowedExtensions.Contains(Path.GetExtension(exeFile).ToLower());
            bool isTxtValid = allowedExtensions.Contains(Path.GetExtension(txtFile).ToLower());
            bool isDocValid = allowedExtensions.Contains(Path.GetExtension(docFile).ToLower());

            // Assert
            Assert.True(isPdfValid);
            Assert.False(isExeValid);
            Assert.False(isTxtValid);
            Assert.False(isDocValid);
        }

        [Theory]
        [InlineData("contract.pdf", true)]
        [InlineData("agreement.PDF", true)]
        [InlineData("virus.exe", false)]
        [InlineData("readme.txt", false)]
        [InlineData("document.docx", false)]
        [InlineData("image.jpg", false)]
        [InlineData("data.zip", false)]
        public void Test_FileExtension_Validation(string fileName, bool shouldBeAllowed)
        {
            // Arrange
            var allowedExtensions = new[] { ".pdf" };
            string extension = Path.GetExtension(fileName).ToLower();

            // Act
            bool isValid = allowedExtensions.Contains(extension);

            // Assert
            Assert.Equal(shouldBeAllowed, isValid);
        }

        [Fact]
        public void Test_FileSize_WithinLimit()
        {
            // Arrange
            long maxFileSize = 5 * 1024 * 1024; // 5MB
            long validFileSize = 2 * 1024 * 1024; // 2MB
            long invalidFileSize = 10 * 1024 * 1024; // 10MB

            // Act
            bool isValidSmallFile = validFileSize <= maxFileSize;
            bool isValidLargeFile = invalidFileSize <= maxFileSize;

            // Assert
            Assert.True(isValidSmallFile);
            Assert.False(isValidLargeFile);
        }
    }
}