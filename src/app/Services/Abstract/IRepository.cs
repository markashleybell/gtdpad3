using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GTDPad.Domain;

namespace GTDPad.Services
{
    public interface IRepository
    {
        Task<IEnumerable<Page>> GetPages(Guid ownerID);

        Task<Page> GetPage(Guid id);

        Task PersistPage(Page page);

        Task DeletePage(Guid id);

        Task<IEnumerable<List>> GetLists(Guid pageID);

        Task<List> GetList(Guid id);

        Task PersistList(List list);

        Task DeleteList(Guid id);

        Task<IEnumerable<RichTextBlock>> GetRichTextBlocks(Guid pageID);

        Task<RichTextBlock> GetRichTextBlock(Guid id);

        Task PersistRichTextBlock(RichTextBlock richTextBlock);

        Task DeleteRichTextBlock(Guid id);

        Task<IEnumerable<ImageBlock>> GetImageBlocks(Guid pageID);

        Task<ImageBlock> GetImageBlock(Guid id);

        Task PersistImageBlock(ImageBlock imageBlock);

        Task DeleteImageBlock(Guid id);

        Task<Image> GetImage(Guid id);

        Task PersistImage(Image image, Guid imageBlockID);

        Task DeleteImage(Guid id);

        Task<ListItem> GetListItem(Guid id);

        Task PersistListItem(ListItem listItem, Guid listID);

        Task DeleteListItem(Guid id);

        Task<User> FindUserByEmail(string email);
    }
}
