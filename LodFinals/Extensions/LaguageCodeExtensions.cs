using System;
using System.Linq;
using LodFinals.Definitions.Enums;
using NoTryCatch.Core.Extensions;
using Xamarin.Forms;

namespace LodFinals.Extensions
{
    public static class LaguageCodeExtensions
    {
        public static string GetCode(this LanguageCodeType languageCode)
        {
            switch (languageCode)
            {
                case LanguageCodeType.DeDe:
                    return "de-DE";
                case LanguageCodeType.FrFr:
                    return "fr-FR";
                case LanguageCodeType.EsEs:
                    return "es-ES";
                case LanguageCodeType.EnAu:
                    return "en-AU";
                case LanguageCodeType.EnUs:
                    return "en-US";
                case LanguageCodeType.EnGb:
                    return "en-GB";
                case LanguageCodeType.EnIn:
                    return "en-IN";
                default:
                    return null;
            }
        }

        public static LanguageCodeType GetCodeType(this string languageCode) =>
            Enum.GetValues(typeof(LanguageCodeType)).Cast<LanguageCodeType>().FirstOrDefault(x => x.GetCode() == languageCode);

        public static ImageSource GetFlagImage(this LanguageCodeType languageCode)
        {
            switch (languageCode)
            {
                case LanguageCodeType.DeDe:
                //return "de-DE";
                case LanguageCodeType.FrFr:
                //return "fr-FR";
                case LanguageCodeType.EsEs:
                //return "es-ES";
                case LanguageCodeType.EnAu:
                //return "en-AU";
                case LanguageCodeType.EnUs:
                //return "en-US";
                case LanguageCodeType.EnGb:
                //return "en-GB";
                case LanguageCodeType.EnIn:
                //return "en-IN";
                default:
                    return AppImages.IcFlagUk;
            }
        }
    }
}
