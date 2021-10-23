using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using GTDPad.Domain;
using GTDPad.Support;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace GTDPad.Services
{
    public class SqlServerRepository : IRepository
    {
        private readonly Settings _cfg;

        public SqlServerRepository(
            IOptionsMonitor<Settings> optionsMonitor)
            : this(optionsMonitor?.CurrentValue ?? new Settings())
        {
        }

        public SqlServerRepository(Settings settings) =>
            _cfg = settings;

        public async Task DeleteImage(Guid id) =>
            await WithConnection(async conn => {
                await conn.ExecuteAsync(
                    sql: "DELETE FROM Images WHERE ID = @ID",
                    param: new { ID = id }
                );
            });

        public async Task DeleteImageBlock(Guid id) =>
            await WithConnection(async conn => {
                await conn.ExecuteAsync(
                    sql: "DELETE FROM Sections WHERE ID = @ID",
                    param: new { ID = id }
                );
            });

        public async Task DeleteList(Guid id) =>
            await WithConnection(async conn => {
                await conn.ExecuteAsync(
                    sql: "DELETE FROM Sections WHERE ID = @ID",
                    param: new { ID = id }
                );
            });

        public async Task DeleteListItem(Guid id) =>
            await WithConnection(async conn => {
                await conn.ExecuteAsync(
                    sql: "DELETE FROM ListItems WHERE ID = @ID",
                    param: new { ID = id }
                );
            });

        public async Task DeletePage(Guid id) =>
             await WithConnection(async conn => {
                 await conn.ExecuteAsync(
                     sql: "DELETE FROM Pages WHERE ID = @ID",
                     param: new { ID = id }
                 );
             });

        public async Task DeleteRichTextBlock(Guid id) =>
            await WithConnection(async conn => {
                const string sql = @"
DELETE FROM TextBlocks WHERE ID = @ID;

DELETE FROM Sections WHERE ID = @ID;";

                await conn.ExecuteAsync(
                    sql: sql,
                    param: new { ID = id }
                );
            });

        public async Task<User> FindUserByEmail(string email) =>
            await WithConnection(async conn => {
                return await conn.QuerySingleOrDefaultAsync<User>(
                    sql: "SELECT * FROM Users WHERE Email = @Email",
                    param: new { Email = email }
                );
            });

        public async Task<IEnumerable<Page>> GetPages(Guid ownerID) =>
            await WithConnection(async conn => {
                return await conn.QueryAsync<Page>(
                    sql: "SELECT ID, Owner, Title, Slug, [Order] FROM Pages WHERE Owner = @Owner",
                    param: new { Owner = ownerID }
                );
            });

        public async Task<Image> GetImage(Guid id) =>
            await WithConnection(async conn => {
                const string sql = @"
SELECT
    i.ID,
    i.FileExtension,
    i.[Order]
FROM
    Images i
WHERE
    i.ID = @ID";

                return await conn.QuerySingleOrDefaultAsync<Image>(
                    sql: sql,
                    param: new { ID = id }
                );
            });

        public async Task<ImageBlock> GetImageBlock(Guid id) =>
            await WithConnection(async conn => {
                const string sql = @"
SELECT
    s.ID,
    s.Page,
    s.Owner,
    s.Title,
    s.[Order]
FROM
    Sections s
WHERE
    s.ID = @ID";

                return await conn.QuerySingleOrDefaultAsync<ImageBlock>(
                    sql: sql,
                    param: new { ID = id }
                );
            });

        public async Task<List> GetList(Guid id) =>
            await WithConnection(async conn => {
                const string sql = @"
SELECT
    s.ID,
    s.Page,
    s.Owner,
    s.Title,
    s.[Order]
FROM
    Sections s
WHERE
    s.ID = @ID";

                return await conn.QuerySingleOrDefaultAsync<List>(
                    sql: sql,
                    param: new { ID = id }
                );
            });

        public async Task<ListItem> GetListItem(Guid id) =>
            await WithConnection(async conn => {
                const string sql = @"
SELECT
    li.ID,
    li.Text,
    li.[Order]
FROM
    ListItems li
WHERE
    li.ID = @ID";

                return await conn.QuerySingleOrDefaultAsync<ListItem>(
                    sql: sql,
                    param: new { ID = id }
                );
            });

        public async Task<Page> GetPage(Guid id) =>
            await WithConnection(async conn => {
                return await conn.QuerySingleOrDefaultAsync<Page>(
                    sql: "SELECT ID, Owner, Title, Slug, [Order] FROM Pages WHERE ID = @ID",
                    param: new { ID = id }
                );
            });

        public async Task<RichTextBlock> GetRichTextBlock(Guid id) =>
            await WithConnection(async conn => {
                const string sql = @"
SELECT
    s.ID,
    s.Page,
    s.Owner,
    s.Title,
    tb.Text,
    s.[Order]
FROM
    Sections s
INNER JOIN
    TextBlocks tb ON tb.ID = s.ID
WHERE
    s.ID = @ID";

                return await conn.QuerySingleOrDefaultAsync<RichTextBlock>(
                    sql: sql,
                    param: new { ID = id }
                );
            });

        public async Task PersistImage(Image image, Guid imageBlockID) =>
            await WithConnection(async conn => {
                const string insertSql = @"
INSERT INTO Images
    (ID, ImageBlock, FileExtension, [Order])
VALUES
    (@ID, @ImageBlock, @FileExtension, @Order);";

                const string updateSql = @"
UPDATE Images SET FileExtension = @FileExtension, [Order] = @Order WHERE ID = @ID;
";

                var existing = await GetImage(image.ID);

                var sql = existing is null
                    ? insertSql
                    : updateSql;

                await conn.ExecuteAsync(
                    sql: sql,
                    param: new {
                        image.ID,
                        ImageBlock = imageBlockID,
                        image.FileExtension,
                        image.Order
                    }
                );
            });

        public async Task PersistImageBlock(ImageBlock imageBlock) =>
            await WithConnection(async conn => {
                const string insertSql = @"
INSERT INTO Sections
    (ID, Owner, Page, Title, Type, [Order])
VALUES
    (@ID, @Owner, @Page, @Title, 2, @Order);";

                const string updateSql = @"
UPDATE Sections SET Title = @Title, Page = @Page, [Order] = @Order WHERE ID = @ID;
";

                var existing = await GetImageBlock(imageBlock.ID);

                var sql = existing is null
                    ? insertSql
                    : updateSql;

                await conn.ExecuteAsync(
                    sql: sql,
                    param: new {
                        imageBlock.ID,
                        imageBlock.Owner,
                        imageBlock.Page,
                        imageBlock.Title,
                        imageBlock.Order
                    }
                );
            });

        public async Task PersistList(List list) =>
            await WithConnection(async conn => {
                const string insertSql = @"
INSERT INTO Sections
    (ID, Owner, Page, Title, Type, [Order])
VALUES
    (@ID, @Owner, @Page, @Title, 3, @Order);";

                const string updateSql = @"
UPDATE Sections SET Title = @Title, Page = @Page, [Order] = @Order WHERE ID = @ID;
";

                var existing = await GetList(list.ID);

                var sql = existing is null
                    ? insertSql
                    : updateSql;

                await conn.ExecuteAsync(
                    sql: sql,
                    param: new {
                        list.ID,
                        list.Owner,
                        list.Page,
                        list.Title,
                        list.Order
                    }
                );
            });

        public async Task PersistListItem(ListItem listItem, Guid listID) =>
            await WithConnection(async conn => {
                const string insertSql = @"
INSERT INTO ListItems
    (ID, List, Text, [Order])
VALUES
    (@ID, @List, @Text, @Order);";

                const string updateSql = @"
UPDATE ListItems SET Text = @Text, [Order] = @Order WHERE ID = @ID;
";

                var existing = await GetListItem(listItem.ID);

                var sql = existing is null
                    ? insertSql
                    : updateSql;

                await conn.ExecuteAsync(
                    sql: sql,
                    param: new {
                        listItem.ID,
                        List = listID,
                        listItem.Text,
                        listItem.Order
                    }
                );
            });

        public async Task PersistPage(Page page) =>
            await WithConnection(async conn => {
                var existing = await GetPage(page.ID);

                var sql = existing is null
                    ? "INSERT INTO Pages (ID, Owner, Title, Slug, [Order]) VALUES (@ID, @Owner, @Title, @Slug, @Order)"
                    : "UPDATE Pages SET Title = @Title, Slug = @Slug, [Order] = @Order WHERE ID = @ID";

                await conn.ExecuteAsync(
                    sql: sql,
                    param: new {
                        page.ID,
                        page.Owner,
                        page.Title,
                        page.Slug,
                        page.Order
                    }
                );
            });

        public async Task PersistRichTextBlock(RichTextBlock richTextBlock) =>
            await WithConnection(async conn => {
                const string insertSql = @"
INSERT INTO Sections
    (ID, Owner, Page, Title, Type, [Order])
VALUES
    (@ID, @Owner, @Page, @Title, 1, @Order);

INSERT INTO TextBlocks
    (ID, Text)
VALUES
    (@ID, @Text);";

                const string updateSql = @"
UPDATE Sections SET Title = @Title, Page = @Page, [Order] = @Order WHERE ID = @ID;

UPDATE TextBlocks SET Text = @Text WHERE ID = @ID;
";

                var existing = await GetRichTextBlock(richTextBlock.ID);

                var sql = existing is null
                    ? insertSql
                    : updateSql;

                await conn.ExecuteAsync(
                    sql: sql,
                    param: new {
                        richTextBlock.ID,
                        richTextBlock.Owner,
                        richTextBlock.Page,
                        richTextBlock.Title,
                        richTextBlock.Text,
                        richTextBlock.Order
                    }
                );
            });

        public async Task<IEnumerable<List>> GetLists(Guid pageID) =>
            await WithConnection(async conn => {
                return await conn.QueryAsync<List>(
                    sql: "SELECT ID, Page, Owner, Title, [Order] FROM Sections WHERE Page = @Page AND Type = 3",
                    param: new { Page = pageID }
                );
            });

        public async Task<IEnumerable<RichTextBlock>> GetRichTextBlocks(Guid pageID) =>
            await WithConnection(async conn => {
                const string sql = @"
SELECT
    s.ID,
    s.Page,
    s.Owner,
    s.Title,
    tb.Text,
    s.[Order]
FROM
    Sections s
INNER JOIN
    TextBlocks tb ON tb.ID = s.ID
WHERE
    s.Page = @Page
AND
    Type = 1";

                return await conn.QueryAsync<RichTextBlock>(
                    sql: sql,
                    param: new { Page = pageID }
                );
            });

        public async Task<IEnumerable<ImageBlock>> GetImageBlocks(Guid pageID) =>
            await WithConnection(async conn => {
                return await conn.QueryAsync<ImageBlock>(
                    sql: "SELECT ID, Page, Owner, Title, [Order] FROM Sections WHERE Page = @Page AND Type = 2",
                    param: new { Page = pageID }
                );
            });

        private async Task WithConnection(Func<SqlConnection, Task> action)
        {
            using var connection = new SqlConnection(_cfg.ConnectionString);

            await action(connection);
        }

        private async Task<T> WithConnection<T>(Func<SqlConnection, Task<T>> action)
        {
            using var connection = new SqlConnection(_cfg.ConnectionString);

            return await action(connection);
        }
    }
}
