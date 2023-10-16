using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://maxlin0523.github.io/2022/06/15/redis-usage/
    去 NuGet 裝上 StackExchange.Redis
 */

namespace 使用Redis
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            #region 建立 Redis

            // 連線字串
            string ipAddress = "127.0.0.1:6379";
            // 建立連線，不需 using
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(ipAddress);
            // 取得實體
            IDatabase db = redis.GetDatabase();

            #endregion

            #region 字串

            // 設定資料
            string key = "String";
            await db.StringSetAsync(key, "ABC");
            // 取得資料
            string value = await db.StringGetAsync(key);
            // 刪除資料
            await db.KeyDeleteAsync(key);

            // 過期時間
            var expiredTime = TimeSpan.FromSeconds(5);

            // 設定過期時間
            var success = await db.KeyExpireAsync(key, expiredTime);

            #endregion

            #region List (集合 Index 都是從 1 開始)

            string listkey = "List";
            // 從頭部加入元素
            await db.ListRightPushAsync(listkey, "AAA");

            // 從尾部加入元素
            await db.ListLeftPushAsync(listkey, "BBB");

            // 取出指定 Index 元素
            var indexValue = await db.ListGetByIndexAsync(listkey, 1);  //回傳 AAA

            // 取出陣列
            var array = (await db.ListRangeAsync(listkey)).ToList();

            // 取出子陣列，取 index 1 之後的 2 個
            var subArray = (await db.ListRangeAsync(listkey, 1, 2)).ToList(); // 取到 AAA 和 BBB

            // 插入多筆資料
            var insertCount = await db.ListRightPushAsync(listkey, new RedisValue[] { "A", "B", "B", "B", "D" });

            // 刪除特定資料，將 A 的資料全部刪除
            await db.ListRemoveAsync(listkey, "A", 0);

            // 刪除特定資料，從頭開始刪除 1 筆 B 的資料
            await db.ListRemoveAsync(listkey, "B", 1);

            // 刪除特定資料，從尾開始刪除 2 筆 B 的資料
            await db.ListRemoveAsync(listkey, "B", -2);

            // 保留一段範圍的資料，取 index 1 之後的 2 個
            await db.ListTrimAsync(listkey, 1, 2);

            #endregion

            #region Set (1個不重複的集合，遇到有重複就忽略掉)

            var setKey = "set";
            // 建立一筆資料
            await db.SetAddAsync(setKey, "1");
            // 建立多筆資料
            await db.SetAddAsync(setKey, new RedisValue[] { 1, 1, 2, 3 });  // 1 只會出現1筆才對(如果不重複的話)

            // 刪除指定元素 1 的資料
            await db.SetRemoveAsync(setKey, 1);

            // 取得 Set 長度
            var setLength = await db.SetLengthAsync(setKey);

            // 取得 Set 所有元素
            var setItems = await db.SetMembersAsync(setKey);

            // 判斷指定元素 1 是否存在
            var itemExists = await db.SetContainsAsync(setKey, 1);

            #endregion

            #region HashSet (像是 Dictionary 的格式)

            var hashKey = "hashData";
            // 新增資料
            await db.HashSetAsync(hashKey, new HashEntry[]
            {
                new HashEntry ("No", "1"),
                new HashEntry ("Name", "ABC"),
                new HashEntry ("Email", "abc@gmail.com")
            });

            // 查看指定資料的所有值
            var hashSetValues = await db.HashValuesAsync(hashKey);

            // 查看指定資料的所有 Key
            var hashSetKeys = await db.HashKeysAsync(hashKey);

            #endregion
        }
    }
}
