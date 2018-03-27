﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduK.Web.Components.TagHelpers
{
    public class UsuarioPanelTagHelper: TagHelper
    {
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public string Nome { get; set; }
        public string Cargo { get; set; }
        public string Foto { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "user-profile");
            output.Attributes.SetAttribute("style", "background: url('data:image/jpeg;base64,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') no-repeat;");
            output.TagMode = TagMode.StartTagAndEndTag;

            string html = $@"
                <div class=""profile-img""> <img src=""{Foto}"" alt=""{Nome}"" /></div>
                <div class=""profile-text"">
                    <a href=""#"" class=""dropdown-toggle u-dropdown"" data-toggle=""dropdown"" role=""button"" aria-haspopup=""true"" aria-expanded=""true"">
                        {Nome}<br />
                        <small>{Cargo}</small>
                    </a>

                    <div class=""dropdown-menu animated flipInY"">
                        <a href=""/Account/Perfil"" class=""dropdown-item""><i class=""ti-user""></i> Meu Perfil</a> 
                        <a href=""/My/Mensagens"" class=""dropdown-item""><i class=""ti-email""></i> Mensagens</a>
                        <div class=""dropdown-divider""></div> 
                        <a href=""/Account/Logout"" class=""dropdown-item""><i class=""fa fa-power-off""></i> Logout</a>
                    </div>
                </div>
            ";

            output.Content.SetHtmlContent(html);
        }
    }
}