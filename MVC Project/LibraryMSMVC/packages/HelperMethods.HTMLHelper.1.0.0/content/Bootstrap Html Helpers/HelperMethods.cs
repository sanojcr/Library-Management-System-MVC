using System.Globalization;
using System.Linq.Expressions;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// This class have methods that will help to manipulate HTML Helpers.
    /// </summary>
    public class Helpers
    {
        /// <summary>
        /// Method that will execute Attributes.Add for each given value in the attributes, then insert them into the specified tag.
        /// </summary>
        /// <param name="Tag">The tag to apply the attributes.</param>
        /// <param name="Attributes">The attributes to be added to the tag. Attention: Separate the key from the value with an equal (=) character.</param>
        public static void AddAtrributes(TagBuilder Tag, params string[] Attributes)
        {
            try
            {
                // Adds all other attributes.
                foreach (var attr in Attributes)
                {
                    Tag.Attributes.Add(attr.Split('=')[0], attr.Split('=')[1]);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This will get the current Culture specified under the system.web/globalization in the Web.Config.
        /// </summary>
        /// <returns></returns>
        public static CultureInfo GetCurrentCulture()
        {
            try
            {
                // Get the globalization section under system.web in Web.Config.
                var section = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/").GetSection("system.web/globalization") as System.Web.Configuration.GlobalizationSection;

                // If the section doesn't exist in the Web.Config, returns null.
                if (string.IsNullOrEmpty(section.Culture))
                {
                    return null;
                }

                return new CultureInfo(section.Culture);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Method that will search for the Validation properties in the metadata, then insert them into the specified tag.
        /// </summary>
        /// <param name="self">The helper itself.</param>
        /// <param name="expression">Model's expression.</param>
        /// <param name="metadata">The metadata from the model.</param>
        /// <param name="tag">The tag to be validated.</param>
        public static void AddValidationProperties<TModel, TValue>(HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, ModelMetadata metadata, TagBuilder tag)
        {
            var fieldName = ExpressionHelper.GetExpressionText(expression);
            var fullBindingName = self.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var fieldId = TagBuilder.CreateSanitizedId(fullBindingName);

            var validationAttributes = self.GetUnobtrusiveValidationAttributes(fullBindingName, metadata);
            foreach (var key in validationAttributes.Keys)
            {
                tag.Attributes.Add(key, validationAttributes[key].ToString());
            }
        }
    }
}