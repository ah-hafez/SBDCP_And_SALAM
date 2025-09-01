using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class LinkMapper
    {
        public static Link Map(LinkAddDTO linkAddDTO)
        {
            if (linkAddDTO == null)
                return null;

            //TransactionCategories transactionCategories = TransactionCategoryMapper.Map(linkAddDTO.TransactionCategories);

            Link link = new Link()
            {
                TransactionCategories = TransactionCategories.Inbound | TransactionCategories.InternalOutbound | TransactionCategories.Outbound | TransactionCategories.DraftOutbound,
                LocalizationIdentifier = LocalizationIdentifierMapper.Map(linkAddDTO.Description)
            };

            return link;
        }

        public static Link Map(LinkEditDTO linkEditDTO)
        {
            if (linkEditDTO == null)
                return null;

            //TransactionCategories transactionCategories = TransactionCategoryMapper.Map(linkEditDTO.TransactionCategories);

            Link link = new Link()
            {
                Id = linkEditDTO.Id,
                TransactionCategories = TransactionCategories.Inbound | TransactionCategories.InternalOutbound | TransactionCategories.Outbound | TransactionCategories.DraftOutbound,
                LocalizationIdentifier = linkEditDTO.Description != null ? LocalizationIdentifierMapper.Map(linkEditDTO.Description) : null,

            };

            return link;
        }

        public static LinkEditDTO Map(Link link, string cultureName)
        {
            if (link == null)
                return null;

            LinkEditDTO linkEditDTO = new LinkEditDTO()
            {
                Id = link.Id,

                Description = link.LocalizationIdentifier.Localizations != null ? LocalizationIdentifierMapper.Map(link.LocalizationIdentifier.Localizations) : null,
                TransactionCategories = TransactionCategoryMapper.Map(link.TransactionCategories, cultureName),
                IsActive = link.IsActive,
                IsLocked = link.IsLocked,
                LockedBy = link.LockedBy,
            };

            return linkEditDTO;
        }

        public static List<LinkDTO> Map(IList<Link> links, string cultureName)
        {
            if (links == null || !links.Any())
            {
                return null;
            }
            List<LinkDTO> linkDTOs = links.Select(linkDTO => new LinkDTO()
            {
                Id = linkDTO.Id,
                LocalName = linkDTO.Text,
                TransactionCategories = TransactionCategoryMapper.Map(linkDTO.TransactionCategories, cultureName),
                Description = linkDTO.LocalizationIdentifier != null ? LocalizationIdentifierMapper.Map(linkDTO.LocalizationIdentifier.Localizations) : null,
                IsActive = linkDTO.IsActive,
                IsLocked = linkDTO.IsLocked,
                LockedBy = linkDTO.LockedBy

            }).ToList();

            return linkDTOs;
        }

        public static List<Link> Map(IList<LinkDTO> linkDTOs, string cultureName)
        {
            if (linkDTOs == null || !linkDTOs.Any())
            {
                return null;
            }
            List<Link> links = linkDTOs.Select(link => new Link()
            {
                Id = link.Id,
                Text = link.LocalName,
                TransactionCategories = TransactionCategories.Inbound | TransactionCategories.InternalOutbound | TransactionCategories.Outbound | TransactionCategories.DraftOutbound,
                LocalizationIdentifier = link.Description != null ? LocalizationIdentifierMapper.Map(link.Description) : null,
            }).ToList();

            return links;
        }
    }
}