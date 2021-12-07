using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

public class Tests
{
    private Server _server;
    private Client _client;
    private Stream _fileStream;
    private CancellationToken _cancellationToken;

    [SetUp]
    public void Setup()
    {
        _server = new Server("127.0.0.1", 80);
        _client = new Client("127.0.0.1", 80);
        _fileStream = new MemoryStream();
        _cancellationToken = new ();
        _server.StartServer();
    }

    [TearDown]
    public void TearDown()
    {
        _server.StopServer();
    }

    [Test]
    public void GetInvalidFileNameTest()
    {
        Assert.ThrowsAsync<AggregateException>(() => Task.Run(() =>  _client.Get("Text.txt", _fileStream, _cancellationToken).Wait()));
    }

    [Test]
    public async Task ListInvalidFileNameTest()
    { 
        Assert.ThrowsAsync<AggregateException>(() => Task.Run(() => _client.Get("Text.txt", _fileStream, _cancellationToken).Wait()));
    }

    [Test]
    public async Task ListTest()
    {
        var data = await _client.List("../../../TestData", _cancellationToken);
        Assert.AreEqual("../../../TestData\\Data.txt", data[0].name);
        Assert.AreEqual(false, data[0].isDir);
    }

    [Test]
    public async Task GetTest()
    {
        var destination = "../../../TestData/Data1.txt";
        var pathForFile = "../../../TestData/Data.txt";
        var result = File.ReadAllBytes(pathForFile);
        using (var fstream = new FileStream(destination, FileMode.OpenOrCreate))
        { 
            var data = await _client.Get(pathForFile, fstream, _cancellationToken);
            Assert.AreEqual(result.Length, data);
        }
        
        var result2 = File.ReadAllBytes(destination);
        Assert.AreEqual(result, result2);
        File.Delete(destination);
    }
}