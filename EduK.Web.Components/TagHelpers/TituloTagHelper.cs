using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EduK.Web.Components.TagHelpers
{
    public class TituloTagHelper : TagHelper
    {
        public string Texto { get; set; }
        public string Descricao { get; set; }
        public string Caminho { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "row page-titles");
            output.TagMode = TagMode.StartTagAndEndTag;

            string htmlDescricao = "";
            if (!string.IsNullOrEmpty(Descricao))
            {
                htmlDescricao = $"<span>{Descricao}</span>";
            }

            string htmlCamimho = "";
            if (!string.IsNullOrEmpty(Caminho))
            {
                htmlCamimho = @"
                    <div class=""col-md-6 col-6 align-self-center"">
                        <div class=""d-flex m-t-10 justify-content-end"">
                            <ol class=""breadcrumb"">
                    ";
                string[] lista = Caminho.Split(",");
                for (int i = 0; i < lista.Length - 1; i++)
                {
                    var elementos = lista[i].Split("|");
                    if (elementos.Count() == 1)
                    {
                        htmlCamimho += $"<li class=\"breadcrumb-item\">{elementos[0]}</li>";
                    }
                    else
                    {
                        htmlCamimho += $"<li class=\"breadcrumb-item\"><a href=\"{elementos[1]}\">{elementos[0]}</a></li>";
                    }
                }
                htmlCamimho += $"<li class=\"breadcrumb-item active\">{lista[lista.Length - 1]}</li>";
                htmlCamimho += @"</ol></div></div>";
            }

            string html = $@"
                <div class=""col-md-6 col-6 align-self-center"">
                    <h3 class=""text-themecolor"">{Texto}</h3>
                    {htmlDescricao}
                </div>
                {htmlCamimho}
            ";

            output.Content.SetHtmlContent(html);
        }

    }
}
