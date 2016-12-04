using System;
using System.Threading.Tasks;
using K2Server;
using K2Server.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace K2UnitTest
{

    [TestClass]
    public class MessageClassUnitTest
    {
        [TestMethod]
        public void MessageValidationTest()
        {
            var m = new Message("PUT\nkey1\nvalue1");
            Assert.AreEqual(m.IsValid(), true);
            //m = new Message("GET\nkey1\nvalue1\nkey2");
            //Assert.AreEqual(m2.IsValid, true, "");
        }

        [TestMethod]
        public void MessageSerializationTest()
        {
            var m1 = new Message("PUT\nkey1\nvalue1");
            var m2 = new Message("PUT\nkey1\nvalue1");
            var json1 = m1.Serialize();
            string json2 = m2.Serialize();
            Assert.AreEqual(json2, json1);
        }
    }

    [TestClass]
    public class K2CacheUnitTest
    {
        [TestMethod]
        public void K2CacheMemorySizeTest()
        {
            K2CacheServer k2 = new K2CacheServer(100);
            for (int i = 0; i < 110; i++)
                k2.Put(i.ToString(), i.ToString());
            Assert.AreEqual(k2.MemorySize >= k2.NumberOfItem, true);
            k2.Put("key1", "value1");
            k2.Put("key1", "value2");
            Assert.AreEqual(k2.Get("key1") == "value2", true);
        }

        [TestMethod]
        public void K2CacheMemoryPreformTest1()
        {
            K2CacheServer k2 = new K2CacheServer(100);
            var m = new Message("GET\nkey1\nvalue1\nkey2");
            var r = k2.Perform(m);
            Assert.AreEqual(r.Result == Results.FAILD.ToString(), true);
            m = new Message("PUT\nkey1\nvalue1");
            r = k2.Perform(m);
            Assert.AreEqual(r.Result == Results.OK.ToString(), true);
            Assert.AreEqual(k2.NumberOfItem == 1, true);

        }

        [TestMethod]
        public void K2CacheMemoryPreformTest2()
        {
            K2CacheServer k2 = new K2CacheServer(100);
            var txt = "{\"CommandName\":\"PUT\",\"Key\":\"key1\",\"Value\":\"value1\",\"Result\":null}";
            var m = Message.Deserialize(txt);
            var r = k2.Perform(m);
            Assert.AreEqual(r.Result, Results.OK.ToString());
        }

        [TestMethod]
        public void K2ClientTest()
        {
            K2CacheServer cacheServer = new K2CacheServer(1000);
            K2Host host = new K2Host(cacheServer)
            {
                IpAddress = "127.0.0.1",
                Port = "3000"
            };
            var task = Task.Run(async () => { await host.StartAsync(); });
            task.Wait();

            K2Client k2c = new K2Client("127.0.0.1", "3000");
            var r = k2c.PutInCache("k1", "v1");
            var v = k2c.GetFromCache("k1");
            Assert.AreEqual(r, Results.OK.ToString());
            Assert.AreEqual(v, "v1");
        }
    }
}
