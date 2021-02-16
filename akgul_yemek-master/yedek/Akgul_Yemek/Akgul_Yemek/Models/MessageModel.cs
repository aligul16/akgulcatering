using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akgul_Yemek.Models
{

    public enum Message_Type
    {
        Success,
        Error,
        Information,
        Warning
    }

    public class MessageModel
    {
        public Message_Type type { get; set; }
        public string caption { get; set; }
        public string message { get; set; }

        public MessageModel(string p_caption,string p_message,Message_Type p_type=Message_Type.Information)
        {
            caption = p_caption;
            type = p_type;
            message = p_message;
        }


        public string MessageTypeToString()
        {
            string message_type_css_string = "";
            string _return_type = "";
            switch (type)
            {
                case Message_Type.Error:
                    message_type_css_string = " alert-danger";
                    _return_type = "warning";
                    break;

                case Message_Type.Success:
                    message_type_css_string = " alert-success";
                    _return_type = "success";
                    break;

                case Message_Type.Information:
                    message_type_css_string = " alert-info";
                    _return_type = "info";
                    break;

                case Message_Type.Warning:
                    message_type_css_string = " alert-warning";
                    _return_type = "warning";
                    break;

                default:
                    message_type_css_string = " alert-info";
                    _return_type = "info";
                    break;
            }

            message_type_css_string=message_type_css_string+"";
            return _return_type;
        }
    }
}