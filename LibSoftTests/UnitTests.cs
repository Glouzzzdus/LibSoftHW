using Documents;
using Documents.Caching;
using Documents.Storage;
using Moq;
using Xunit;

namespace LibSoftTests;
public class DocumentStorageTest
{
    [Fact]
    public void GetByNumber_CallsCache_TryGetValue_WithCorrectKey()
    {
        // Arrange
        var expectedKey = "12345";
        var mockCache = new Mock<ICache>();
        var mockFileReader = new Mock<IFileReader>();
        var mockDirectoryHelper = new Mock<IDirectoryHelper>();

        mockCache.Setup(x => x.TryGetValue(It.IsAny<string>(), out It.Ref<Document>.IsAny))
                 .Returns(false);

        mockDirectoryHelper.Setup(x => x.GetFiles(It.IsAny<string>()))
                           .Returns(new string[] { "mockFilePath" });
        
        mockFileReader.Setup(x => x.ReadAllText(It.IsAny<string>()))
                      .Returns("{\"type\":\"Book\"}");

        IDocumentStorage storage =
            new FileDocumentStorage(mockCache.Object, "mock_path", mockFileReader.Object, mockDirectoryHelper.Object);

        // Act
        List<Document> actualDocuments = storage.GetByNumber(expectedKey);

        // Assert
        mockCache.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<Document>(), It.IsAny<DateTimeOffset>()), Times.Once);
    }

    [Fact]
    public void GetByNumber_ReturnsEmptyList_WhenCacheNotFound()
    {
        // Arrange
        string expectedKey = "12345";
        var mockCache = new Mock<ICache>();
        mockCache.Setup(x => x.TryGetValue(expectedKey, out It.Ref<Document>.IsAny)).Returns(false);

        IDocumentStorage storage = new FileDocumentStorage(mockCache.Object);

        // Act
        List<Document> actualDocuments = storage.GetByNumber(expectedKey);

        // Assert
        Assert.Empty(actualDocuments);
    }

    [Fact]
    public void GetByNumber_ReturnsDocument_WhenCacheFound()
    {
        // Arrange
        string expectedKey = "12345";
        var mockCache = new Mock<ICache>();
        Book expectedDocument = new Book
        {
            Title = "Test Title",
            Authors = new List<string> { "Author1", "Author2" },
            DatePublished = DateTime.Now,
            ISBN = "123456789",
            NumberOfPages = 100,
            Publisher = "Test Publisher"
        };
        mockCache.Setup(x => x.TryGetValue(expectedKey, out It.Ref<Document>.IsAny))
            .Callback((string key, out Document value) => value = expectedDocument)
            .Returns(true);

        IDocumentStorage storage = new FileDocumentStorage(mockCache.Object);

        // Act
        List<Document> actualDocuments = storage.GetByNumber(expectedKey);

        // Assert
        Assert.Single(actualDocuments);
        Assert.Equal(expectedDocument.Title, actualDocuments[0].Title);
        Assert.IsType<Book>(actualDocuments[0]);
    }

    [Fact]
    public void GetByNumber_CallsCache_Add_WhenCacheNotFound()
    {
        // Arrange
        string expectedKey = "12345";
        var mockCache = new Mock<ICache>();
        mockCache.Setup(x => x.TryGetValue(expectedKey, out It.Ref<Document>.IsAny)).Returns(false);

        IDocumentStorage storage = new FileDocumentStorage(mockCache.Object);

        // Act
        List<Document> actualDocuments = storage.GetByNumber(expectedKey);

        // Assert
        mockCache.Verify(x => x.Add(expectedKey, It.IsAny<Document>(), It.IsAny<DateTimeOffset>()), Times.Once);
    }

    [Fact]
    public void GetByNumber_DoesNotCallCache_Add_WhenCacheFound()
    {
        // Arrange
        string expectedKey = "12345";
        var mockCache = new Mock<ICache>();
        Book expectedDocument = new Book
        {
            Title = "Test Title",
            Authors = new List<string> { "Author1", "Author2" },
            DatePublished = DateTime.Now,
            ISBN = "123456789",
            NumberOfPages = 100,
            Publisher = "Test Publisher"
        };
        mockCache.Setup(x => x.TryGetValue(expectedKey, out It.Ref<Document>.IsAny))
            .Callback((string key, out Document value) => value = expectedDocument)
            .Returns(true);

        IDocumentStorage storage = new FileDocumentStorage(mockCache.Object);

        // Act
        List<Document> actualDocuments = storage.GetByNumber(expectedKey);

        // Assert
        mockCache.Verify(x => x.Add(expectedKey, It.IsAny<Document>(), It.IsAny<DateTimeOffset>()), Times.Never);
    }
}
