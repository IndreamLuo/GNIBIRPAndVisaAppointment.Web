using System;
using System.ComponentModel.DataAnnotations;
using GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage;

namespace GNIBIRPAndVisaAppointment.Web.Models
{
    public class InformationModel
    {
        public InformationModel() { }
        
        internal InformationModel(Information entity)
        {
            Key = entity.PartitionKey;
            Language = entity.RowKey;
            Title = entity.Title;
            CreatedTime = entity.CreatedTime;
            Author = entity.Author;
            Content = entity.Content;
            FacebookComment = entity.FacebookComment;
        }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Language { get; set; }

        public string Title { get; set; }

        public DateTime CreatedTime { get; set; }

        public string Author { get; set; }

        public string Content { get; set; }
        
        public bool FacebookComment { get; set; }
    }
}