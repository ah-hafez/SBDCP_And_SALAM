using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace MCS.Domain.NonMappedTypes
{
    public enum PropertyValueType
    {
        Date = 1,
        Time = 2,
        [Display(Name = "Date & Time")]
        Datetime = 3,
        Integer = 4,
        Decimal = 5,
        String = 6,
        [Display(Name = "Multi Line String")]
        MultiLineString = 7,
        List = 8,
        Boolean = 9,
        Url = 10,
        Image = 11,
        Video = 12,
        Year = 13,
        Month = 14,
        Day = 15,
    }

    public class PropertyTypeSetting
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public PropertyValueType Type { get; set; }

        public PropertyTypeSetting(string name, object value, PropertyValueType type)
        {
            Name = name;
            Value = value;
            Type = type;
        }
    }

    public class PropertyType
    {
        public PropertyValueType ValueType { get; set; }

        public virtual bool IsValid(string value)
        {
            return true;
        }

        public virtual List<PropertyTypeSetting> GetSettings()
        {
            return new List<PropertyTypeSetting>();
        }
    }

    public class BooleanProperty : PropertyType
    {
        public BooleanProperty()
        {
            ValueType = PropertyValueType.Boolean;
        }

        public override bool IsValid(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var v = value.ToLower();
                return v == "true" || v == "false";
            }
            return true;
        }
    }

    public class DateProperty : PropertyType
    {
        public DateTime? MinValue { get; set; }
        public DateTime? MaxValue { get; set; }

        public DateProperty()
        {
            ValueType = PropertyValueType.Date;
        }

        public override bool IsValid(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                DateTime d;
                return DateTime.TryParse(value, out d) && (!MinValue.HasValue || MinValue <= d) && (!MaxValue.HasValue || MaxValue >= d);
            }
            return true;
        }

        public override List<PropertyTypeSetting> GetSettings()
        {
            return new List<PropertyTypeSetting>
            {
                new PropertyTypeSetting(nameof(MinValue), MinValue, PropertyValueType.Date),
                new PropertyTypeSetting(nameof(MaxValue), MaxValue, PropertyValueType.Date),
            };
        }
    }

    public class TimeProperty : PropertyType
    {
        public TimeSpan? MinValue { get; set; }
        public TimeSpan? MaxValue { get; set; }

        public TimeProperty()
        {
            ValueType = PropertyValueType.Time;
        }

        public override bool IsValid(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                TimeSpan t;
                return TimeSpan.TryParse(value, out t) && (!MinValue.HasValue || MinValue <= t) && (!MaxValue.HasValue || MaxValue >= t);
            }
            return true;
        }

        public override List<PropertyTypeSetting> GetSettings()
        {
            return new List<PropertyTypeSetting>
            {
                new PropertyTypeSetting(nameof(MinValue), MinValue, PropertyValueType.Time),
                new PropertyTypeSetting(nameof(MaxValue), MaxValue, PropertyValueType.Time),
            };
        }
    }

    public class DatetimeProperty : PropertyType
    {
        public DateTime? MinValue { get; set; }
        public DateTime? MaxValue { get; set; }

        public DatetimeProperty()
        {
            ValueType = PropertyValueType.Datetime;
        }

        public override bool IsValid(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                DateTime d;
                return DateTime.TryParse(value, out d) && (!MinValue.HasValue || MinValue <= d) && (!MaxValue.HasValue || MaxValue >= d);
            }
            return true;
        }

        public override List<PropertyTypeSetting> GetSettings()
        {
            return new List<PropertyTypeSetting>
            {
                new PropertyTypeSetting(nameof(MinValue), MinValue, PropertyValueType.Datetime),
                new PropertyTypeSetting(nameof(MaxValue), MaxValue, PropertyValueType.Datetime),
            };
        }
    }

    public class IntegerProperty : PropertyType
    {
        public long? MinValue { get; set; }
        public long? MaxValue { get; set; }

        public IntegerProperty()
        {
            ValueType = PropertyValueType.Integer;
        }

        public override bool IsValid(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                long i;
                return long.TryParse(value, out i) && (!MinValue.HasValue || MinValue <= i) && (!MaxValue.HasValue || MaxValue >= i);
            }
            return true;
        }

        public override List<PropertyTypeSetting> GetSettings()
        {
            return new List<PropertyTypeSetting>
            {
                new PropertyTypeSetting(nameof(MinValue), MinValue, PropertyValueType.Integer),
                new PropertyTypeSetting(nameof(MaxValue), MaxValue, PropertyValueType.Integer),
            };
        }
    }

    public class DecimalProperty : PropertyType
    {
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }

        public DecimalProperty()
        {
            ValueType = PropertyValueType.Decimal;
        }

        public override bool IsValid(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                decimal d;
                return decimal.TryParse(value, out d) && (!MinValue.HasValue || MinValue <= d) && (!MaxValue.HasValue || MaxValue >= d);
            }
            return true;
        }

        public override List<PropertyTypeSetting> GetSettings()
        {
            return new List<PropertyTypeSetting>
            {
                new PropertyTypeSetting(nameof(MinValue), MinValue, PropertyValueType.Decimal),
                new PropertyTypeSetting(nameof(MaxValue), MaxValue, PropertyValueType.Decimal),
            };
        }
    }

    public class StringProperty : PropertyType
    {
        public StringProperty()
        {
            ValueType = PropertyValueType.String;
        }
    }

    public class MultiLineStringProperty : PropertyType
    {
        public MultiLineStringProperty()
        {
            ValueType = PropertyValueType.MultiLineString;
        }
    }

    public class ListProperty : PropertyType
    {
        public int? MaxValues { get; set; }

        public virtual List<string> Values { get; set; }
        public virtual bool HasTranslation { get; set; }

        public ListProperty()
        {
            ValueType = PropertyValueType.List;
        }

        public override bool IsValid(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var values = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                return values.All(v => Values.Contains(v)) && (!MaxValues.HasValue || values.Count() <= MaxValues);
            }
            return true;
        }

        public override List<PropertyTypeSetting> GetSettings()
        {
            return new List<PropertyTypeSetting>
            {
                new PropertyTypeSetting(nameof(MaxValues), MaxValues, PropertyValueType.Integer),
                new PropertyTypeSetting(nameof(Values), Values, PropertyValueType.List),
            };
        }
    }

    public class YearProperty : ListProperty
    {
        [Range(1, int.MaxValue)]
        public int MinimumYear { get; set; } = 1960;
        public int MaximumYear { get; set; }
        public bool Now { get; set; }

        public override List<string> Values
        {
            get { return Enumerable.Range(MinimumYear, Now ? DateTime.UtcNow.Year : MaximumYear).Select(i => i.ToString()).ToList(); }
            set { }
        }

        public override bool HasTranslation { get { return false; } set { } }

        public YearProperty()
        {
            ValueType = PropertyValueType.Year;
        }

        public override bool IsValid(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var values = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                return values.All(v => Values.Contains(v)) && (!MaxValues.HasValue || values.Count() <= MaxValues);
            }
            return true;
        }

        public override List<PropertyTypeSetting> GetSettings()
        {
            return new List<PropertyTypeSetting>
            {
                new PropertyTypeSetting(nameof(MaxValues), MaxValues, PropertyValueType.Integer),
                new PropertyTypeSetting(nameof(MinimumYear), MinimumYear, PropertyValueType.Integer),
                new PropertyTypeSetting(nameof(MaximumYear), MaximumYear, PropertyValueType.Integer),
                new PropertyTypeSetting(nameof(Now), Now, PropertyValueType.Boolean),
            };
        }
    }

    public class MonthProperty : IntegerProperty
    {
        public MonthProperty()
        {
            ValueType = PropertyValueType.Month;
            MinValue = 1;
            MaxValue = 12;
        }
    }

    public class DayProperty : IntegerProperty
    {
        public DayProperty()
        {
            ValueType = PropertyValueType.Year;
            ValueType = PropertyValueType.Month;
            MinValue = 1;
            MaxValue = 31;
        }
    }

    public class UrlProperty : PropertyType
    {
        public UrlProperty()
        {
            ValueType = PropertyValueType.Url;
        }

        public override bool IsValid(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return Uri.IsWellFormedUriString(value, UriKind.Absolute);
            }
            return true;
        }
    }

    public class ImageProperty : UrlProperty
    {
        public ImageProperty()
        {
            ValueType = PropertyValueType.Image;
        }
    }

    public class VideoProperty : UrlProperty
    {
        public VideoProperty()
        {
            ValueType = PropertyValueType.Video;
        }
    }

    public class PropertyTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(PropertyType).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject item = JObject.Load(reader);
            switch ((PropertyValueType)item["ValueType"].Value<int>())
            {
                case PropertyValueType.Boolean:
                    return item.ToObject<BooleanProperty>();
                case PropertyValueType.Date:
                    return item.ToObject<DateProperty>();
                case PropertyValueType.Datetime:
                    return item.ToObject<DatetimeProperty>();
                case PropertyValueType.Time:
                    return item.ToObject<TimeProperty>();
                case PropertyValueType.Decimal:
                    return item.ToObject<DecimalProperty>();
                case PropertyValueType.Integer:
                    return item.ToObject<IntegerProperty>();
                case PropertyValueType.List:
                    return item.ToObject<ListProperty>();
                case PropertyValueType.String:
                    return item.ToObject<StringProperty>();
                case PropertyValueType.MultiLineString:
                    return item.ToObject<MultiLineStringProperty>();
                case PropertyValueType.Url:
                    return item.ToObject<UrlProperty>();
                case PropertyValueType.Image:
                    return item.ToObject<ImageProperty>();
                case PropertyValueType.Video:
                    return item.ToObject<VideoProperty>();
                default:
                    return item.ToObject<PropertyType>();
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject.FromObject(value).WriteTo(writer);
        }
    }
}
