using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TrayMapper
    {
        public static Tray Map(EditTrayDTO trayEditDTO)
        {
            if (trayEditDTO == null)
            {
                return null;
            }
            Tray tray = new Tray()
            {
                Id = trayEditDTO.Id,
                Name = new Lookup()
            };

            tray.Name.Localizations = LookupLocalizationMapper.Map(trayEditDTO.Names);

            return tray;
        }

        public static EditTrayDTO Map(Tray tray)
        {
            if (tray == null)
            {
                return null;
            }
            EditTrayDTO trayDTO = new EditTrayDTO()
            {
                Id = tray.Id,
                Names = LookupLocalizationMapper.Map(tray.Name.Localizations)
            };

            return trayDTO;
        }

        public static List<TrayDTO> Map(IList<Tray> trays, bool isSelected = false)
        {
            List<TrayDTO> trayDTOs = trays
                .Select(tray => new TrayDTO()
                {
                    Id = tray.Id,
                    IsSelected = isSelected,
                    Sort = tray.Sort,
                    LocalName = tray.LocalName,
                    Names = tray.Name != null ? LookupLocalizationMapper.Map(tray.Name.Localizations) : null
                }).ToList();

            return trayDTOs;
        }

        public static List<Tray> Map(IList<TrayDTO> traysDTO)
        {
            if (traysDTO == null || !traysDTO.Any())
            {
                return null;
            }
            List<Tray> trays = traysDTO
                .Select(item => new Tray()
                {
                    Id = item.Id,
                    Sort = item.Sort,
                }).ToList();

            return trays;
        }
    }
}