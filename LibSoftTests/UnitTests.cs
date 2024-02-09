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
        string expectedKey = "12345";
        var mockCache = new Mock<ICache>();
        mockCache.Setup(x => x.TryGetValue(expectedKey, out It.Ref<Document>.IsAny)).Returns(false); 

        IDocumentStorage storage = new FileDocumentStorage(mockCache.Object); 

        // Act
        List<Document> actualDocuments = storage.GetByNumber(expectedKey);

        // Assert
        mockCache.Verify(x => x.TryGetValue(expectedKey, out It.Ref<Document>.IsAny), Times.Once);
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
        Document expectedDocument = new Document { DocumentNumber = expectedKey, Content = "Content" };
        mockCache.Setup(x => x.TryGetValue(expectedKey, out It.Ref<Document>.IsAny))
            .Callback((string key, out Document value) => value = expectedDocument)
            .Returns(true);

        IDocumentStorage storage = new FileDocumentStorage(mockCache.Object);

        // Act
        List<Document> actualDocuments = storage.GetByNumber(expectedKey);

        // Assert
        Assert.Single(actualDocuments);
        Assert.Equal(expectedDocument.Content, actualDocuments[0].Content);
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
        Document expectedDocument = new Document { DocumentNumber = expectedKey, Content = "Content" };
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
