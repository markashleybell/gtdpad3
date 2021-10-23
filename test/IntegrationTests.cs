using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GTDPad.Domain;
using GTDPad.Services;
using GTDPad.Support;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using g = GTDPad.Domain;

namespace test
{
    public class IntegrationTests : IntegrationTestFixtureBase
    {
        private readonly Settings _testSettings = new Settings {
            ConnectionStrings = new Dictionary<string, string>()
        };

        private readonly User _testUser = new User(
            id: new Guid("df77778f-2ef3-49af-a1a8-b1f064891ef5"),
            email: "testuser@GTDPad.com",
            password: "!gtdpad-test2020"
        );

        public IntegrationTests() =>
            _testSettings.ConnectionStrings.Add("Main", _cfg.GetConnectionString("Test"));

        [Test]
        public async Task Page()
        {
            var repository = new SqlServerRepository(_testSettings);

            var newPage = new Page(
                id: Guid.NewGuid(),
                owner: _testUser.ID,
                title: "PAGE",
                slug: "page",
                order: 10
            );

            await repository.PersistPage(newPage);

            var page = await repository.GetPage(newPage.ID);

            Assert.AreEqual(newPage.ID, page.ID);
            Assert.AreEqual(newPage.Title, page.Title);
            Assert.AreEqual(newPage.Slug, page.Slug);
            Assert.AreEqual(newPage.Order, page.Order);

            var updatedPage = page.With(title: "PAGE UPDATED", slug: "page-updated", order: 20);

            await repository.PersistPage(updatedPage);

            var pageAfterUpdate = await repository.GetPage(updatedPage.ID);

            Assert.AreEqual(page.ID, pageAfterUpdate.ID);
            Assert.AreEqual(updatedPage.Title, pageAfterUpdate.Title);
            Assert.AreEqual(updatedPage.Slug, pageAfterUpdate.Slug);
            Assert.AreEqual(updatedPage.Order, pageAfterUpdate.Order);

            await repository.DeletePage(page.ID);

            var pageAfterDelete = await repository.GetPage(page.ID);

            Assert.AreEqual(default(Page), pageAfterDelete);
        }

        [Test]
        public async Task RichTextBlock()
        {
            var repository = new SqlServerRepository(_testSettings);

            var page = new Page(
                id: Guid.NewGuid(),
                owner: _testUser.ID,
                title: "PAGE",
                slug: "page",
                order: 10
            );

            await repository.PersistPage(page);

            var newRichTextBlock = new RichTextBlock(
                id: Guid.NewGuid(),
                page: page.ID,
                owner: _testUser.ID,
                title: "RICH TEXT BLOCK",
                text: "<p>Rich Text Block Body.</p>",
                order: 10
            );

            await repository.PersistRichTextBlock(newRichTextBlock);

            var richTextBlock = await repository.GetRichTextBlock(newRichTextBlock.ID);

            Assert.AreEqual(newRichTextBlock.ID, richTextBlock.ID);
            Assert.AreEqual(newRichTextBlock.Title, richTextBlock.Title);
            Assert.AreEqual(newRichTextBlock.Text, richTextBlock.Text);
            Assert.AreEqual(newRichTextBlock.Order, richTextBlock.Order);

            var updatedRichTextBlock = richTextBlock.With(
                title: "RICH TEXT BLOCK UPDATED",
                text: "<p>Rich Text Block Body Updated.</p>",
                order: 20
            );

            await repository.PersistRichTextBlock(updatedRichTextBlock);

            var richTextBlockAfterUpdate = await repository.GetRichTextBlock(updatedRichTextBlock.ID);

            Assert.AreEqual(richTextBlock.ID, richTextBlockAfterUpdate.ID);
            Assert.AreEqual(updatedRichTextBlock.Title, richTextBlockAfterUpdate.Title);
            Assert.AreEqual(updatedRichTextBlock.Text, richTextBlockAfterUpdate.Text);
            Assert.AreEqual(updatedRichTextBlock.Order, richTextBlockAfterUpdate.Order);

            await repository.DeleteRichTextBlock(richTextBlock.ID);

            var richTextBlockAfterDelete = await repository.GetRichTextBlock(richTextBlock.ID);

            Assert.AreEqual(default(RichTextBlock), richTextBlockAfterDelete);
        }

        [Test]
        public async Task ImageBlock()
        {
            var repository = new SqlServerRepository(_testSettings);

            var page = new Page(
                id: Guid.NewGuid(),
                owner: _testUser.ID,
                title: "PAGE",
                slug: "page",
                order: 10
            );

            await repository.PersistPage(page);

            var newImageBlock = new ImageBlock(
                id: Guid.NewGuid(),
                page: page.ID,
                owner: _testUser.ID,
                title: "IMAGE BLOCK",
                order: 10
            );

            await repository.PersistImageBlock(newImageBlock);

            var imageBlock = await repository.GetImageBlock(newImageBlock.ID);

            Assert.AreEqual(newImageBlock.ID, imageBlock.ID);
            Assert.AreEqual(newImageBlock.Title, imageBlock.Title);
            Assert.AreEqual(newImageBlock.Order, imageBlock.Order);

            var updatedImageBlock = imageBlock.With(
                title: "IMAGE BLOCK UPDATED",
                order: 20
            );

            await repository.PersistImageBlock(updatedImageBlock);

            var imageBlockAfterUpdate = await repository.GetImageBlock(updatedImageBlock.ID);

            Assert.AreEqual(imageBlock.ID, imageBlockAfterUpdate.ID);
            Assert.AreEqual(updatedImageBlock.Title, imageBlockAfterUpdate.Title);
            Assert.AreEqual(updatedImageBlock.Order, imageBlockAfterUpdate.Order);

            await repository.DeleteImageBlock(imageBlock.ID);

            var imageBlockAfterDelete = await repository.GetImageBlock(imageBlock.ID);

            Assert.AreEqual(default(ImageBlock), imageBlockAfterDelete);
        }

        [Test]
        public async Task List()
        {
            var repository = new SqlServerRepository(_testSettings);

            var page = new Page(
                id: Guid.NewGuid(),
                owner: _testUser.ID,
                title: "PAGE",
                slug: "page",
                order: 10
            );

            await repository.PersistPage(page);

            var newList = new g.List(
                id: Guid.NewGuid(),
                page: page.ID,
                owner: _testUser.ID,
                title: "LIST",
                order: 10
            );

            await repository.PersistList(newList);

            var list = await repository.GetList(newList.ID);

            Assert.AreEqual(newList.ID, list.ID);
            Assert.AreEqual(newList.Title, list.Title);
            Assert.AreEqual(newList.Order, list.Order);

            var updatedList = list.With(
                title: "LIST UPDATED",
                order: 20
            );

            await repository.PersistList(updatedList);

            var listAfterUpdate = await repository.GetList(updatedList.ID);

            Assert.AreEqual(list.ID, listAfterUpdate.ID);
            Assert.AreEqual(updatedList.Title, listAfterUpdate.Title);
            Assert.AreEqual(updatedList.Order, listAfterUpdate.Order);

            await repository.DeleteList(list.ID);

            var listAfterDelete = await repository.GetList(list.ID);

            Assert.AreEqual(default(g.List), listAfterDelete);
        }

        [Test]
        public async Task Image()
        {
            var repository = new SqlServerRepository(_testSettings);

            var page = new Page(
                id: Guid.NewGuid(),
                owner: _testUser.ID,
                title: "PAGE",
                slug: "page",
                order: 10
            );

            await repository.PersistPage(page);

            var imageBlock = new ImageBlock(
                id: Guid.NewGuid(),
                page: page.ID,
                owner: _testUser.ID,
                title: "IMAGE BLOCK",
                order: 10
            );

            await repository.PersistImageBlock(imageBlock);

            var newImage = new Image(
                id: Guid.NewGuid(),
                fileExtension: ".jpg",
                order: 10
            );

            await repository.PersistImage(newImage, imageBlock.ID);

            var image = await repository.GetImage(newImage.ID);

            Assert.AreEqual(newImage.ID, image.ID);
            Assert.AreEqual(newImage.FileExtension, image.FileExtension);
            Assert.AreEqual(newImage.Order, image.Order);

            var updatedImage = image.With(
                fileExtension: ".png",
                order: 20
            );

            await repository.PersistImage(updatedImage, imageBlock.ID);

            var imageAfterUpdate = await repository.GetImage(updatedImage.ID);

            Assert.AreEqual(imageAfterUpdate.ID, newImage.ID);
            Assert.AreEqual(imageAfterUpdate.FileExtension, updatedImage.FileExtension);
            Assert.AreEqual(imageAfterUpdate.Order, updatedImage.Order);

            await repository.DeleteImage(image.ID);

            var imageAfterDelete = await repository.GetImage(image.ID);

            Assert.AreEqual(default(Image), imageAfterDelete);
        }

        [Test]
        public async Task ListItem()
        {
            var repository = new SqlServerRepository(_testSettings);

            var page = new Page(
                id: Guid.NewGuid(),
                owner: _testUser.ID,
                title: "PAGE",
                slug: "page",
                order: 10
            );

            await repository.PersistPage(page);

            var list = new g.List(
                id: Guid.NewGuid(),
                page: page.ID,
                owner: _testUser.ID,
                title: "LIST",
                order: 10
            );

            await repository.PersistList(list);

            var newListItem = new ListItem(
                id: Guid.NewGuid(),
                text: "LIST ITEM",
                order: 10
            );

            await repository.PersistListItem(newListItem, list.ID);

            var listItem = await repository.GetListItem(newListItem.ID);

            Assert.AreEqual(newListItem.ID, listItem.ID);
            Assert.AreEqual(newListItem.Text, listItem.Text);
            Assert.AreEqual(newListItem.Order, listItem.Order);

            var updatedListItem = listItem.With(
                text: "LIST ITEM UPDATED",
                order: 20
            );

            await repository.PersistListItem(updatedListItem, list.ID);

            var listItemAfterUpdate = await repository.GetListItem(updatedListItem.ID);

            Assert.AreEqual(listItemAfterUpdate.ID, newListItem.ID);
            Assert.AreEqual(listItemAfterUpdate.Text, updatedListItem.Text);
            Assert.AreEqual(listItemAfterUpdate.Order, updatedListItem.Order);

            await repository.DeleteListItem(listItem.ID);

            var listItemAfterDelete = await repository.GetListItem(listItem.ID);

            Assert.AreEqual(default(ListItem), listItemAfterDelete);
        }
    }
}
