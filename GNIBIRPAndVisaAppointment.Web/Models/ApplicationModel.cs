using System.ComponentModel.DataAnnotations;

namespace GNIBIRPAndVisaAppointment.Web.Models
{
    public class ApplicationModel
    {
        public ApplicationModel() { }

        public ApplicationModel(GNIBIRPAndVisaAppointment.Web.DataAccess.Model.Storage.Application application)
        {

        }

        public string Id { get; set; }

        public bool IsInitialized { get; set; }
        
        [Required]
        public string Category { get; set; }
        
        [Required]
        public string SubCategory { get; set; }
        public bool HasGNIB { get; set; }

        [MaxLength(20)]
        public string GNIBNo { get; set; }
        public string GNIBExDT { get; set; }
        
        [Required]
        public bool UsrDeclaration { get; set; }
        
        [Required]
        public string Salutation { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string GivenName { get; set; }
        
        [MaxLength(200)]
        public string MidName { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string SurName { get; set; }
        
        [Required]
        public string DOB { get; set; }
        
        [Required]
        public string Nationality { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Compare("Email")]
        public string EmailConfirm { get; set; }
        public bool IsFamily { get; set; }
        public string FamAppNo { get; set; }
        public bool HasPassport { get; set; }
        [MaxLength(30)]
        public string PPNo { get; set; }
        public string PPReason { get; set; }
        public string Comment { get; set; }
        
        [Required]
        public bool AuthorizeDataUsage { get; set; }

        public static string[] Salutations = new []
        {
            "Mr",
            "Mrs",
            "Miss",
            "Ms",
            "Dr"
        };

        public static string[] Nationalities = new []
        {
            "Afghanistan, Islamic Republic of",
            "Albania, Republic of",
            "Algeria, People's Democratic Republic of",
            "Angola, Republic of",
            "Anguilla",
            "Antigua and Barbuda",
            "Argentina",
            "Armenia, Republic of",
            "Aruba",
            "Australia",
            "Azerbaijan, Republic of",
            "Bahamas",
            "Bahrain, Kingdom of",
            "Bangladesh",
            "Bangladesh, People's Republic of",
            "Barbados",
            "Belarus, Republic of",
            "Belize",
            "Benin, Republic of",
            "Bermuda",
            "Bhutan, Kingdom of",
            "Bolivia",
            "Bosnia and Herzegovina",
            "Botswana",
            "Brazil",
            "British Virgin Islands",
            "Brunei",
            "Burkina Faso",
            "Burma/Myanmar",
            "Burma/Republic of the Union of Myanmar",
            "Burundi, Republic of",
            "Cambodia, Kingdom of",
            "Cameroon, Republic of",
            "Canada",
            "Cape Verde, Republic of",
            "Cayman Islands",
            "Central African Republic",
            "Chad, Republic of",
            "Chile",
            "China, People's Republic of",
            "Colombia, Republic of",
            "Comoros, Union of the",
            "Congo, Democratic Republic of",
            "Congo, Republic of (Brazzaville)",
            "Costa Rica",
            "Cote d'Ivoire, republic of ivory coast",
            "Cuba, Republic of",
            "Djibouti, Republic of",
            "Dominica",
            "Dominican Republic",
            "Ecuador, Republic of",
            "Egypt, arab republic of",
            "El Salvador",
            "Equatorial Guinea, Republic of",
            "Eritrea, State of",
            "Ethiopia, Federal democratic republic of",
            "Falkland Islands",
            "Fiji",
            "Gabon, Republic of",
            "Gambia, Republic of",
            "Georgia",
            "Ghana, Republic of",
            "Greenland",
            "Grenada",
            "Guatemala",
            "Guinea-Bissau, Republic of",
            "Guinea, Republic of",
            "Guyana",
            "Haiti, Republic of",
            "Honduras",
            "Hong Kong",
            "India, Republic of",
            "Indonesia, Republic of",
            "Iran, Islamic Republic of",
            "Iraq, Republic of",
            "Israel",
            "Jamaica",
            "Japan",
            "Jordan, Hashemite Kingdom of",
            "Kazakhstan, Republic of",
            "Kenya, Republic of",
            "Kiribati",
            "Korea (North), Democratic Republic of",
            "Kosovo, Republic of",
            "Kuwait, State of",
            "Kyrgyzstan",
            "Laos People's Republic of",
            "Lebanon, Republic of",
            "Lesotho",
            "Liberia, Republic of",
            "Libya, State of",
            "Macao",
            "Macedonia, The former Yuoslav Republic of",
            "Madagascar, Republic of",
            "Malawi",
            "Malaysia",
            "Maldives",
            "Mali, Republic of",
            "Marshall Islands, Republic of",
            "Mauritania, Islamic Republic of",
            "Mauritius, Republic of",
            "Mexico",
            "Micronesia, Federated States of",
            "Moldova, Republic of",
            "Mongolia",
            "Montenegro",
            "Montserrat",
            "Morocco, Kingdom of",
            "Mozambique, Republic of",
            "Namibia, Republic of",
            "Nauru",
            "Nepal, Federal Democratic Republic of",
            "New Zealand",
            "Nicaragua",
            "Nigeria, Federal Republic of",
            "Niger, Republic of",
            "Oman, Sultanate of",
            "Pakistan, Islamic Republic of",
            "Palau, Republic of",
            "Palestinian National Authority",
            "Panama",
            "Papua New Guinea",
            "Paraguay",
            "Peru, republic of",
            "Philippines, Republic of",
            "Pitcairn Island",
            "Qatar, State of",
            "Russian Federation",
            "Rwanda, Republic of",
            "Saint-Barthelemy",
            "Samoa",
            "Sao Tome and Principe, Democratic Republic of",
            "Saudi Arabia, Kingdom of",
            "Senegal, Republic of",
            "Serbia, Republic of",
            "Seychelles",
            "Sierra Leone, Republic of",
            "Singapore",
            "Solomon Islands",
            "Somalia, Federal Republic of",
            "South Africa",
            "South Korea",
            "Sri Lanka, Democratic Socialist Republic of",
            "St Helena, Ascension and Tristan da Cunha",
            "St Kitts and Nevis",
            "St Lucia",
            "St Martin",
            "St Vincent and The Grenadines",
            "Sudan, Republic of the",
            "Suriname, Republic of",
            "Swaziland",
            "Syrian Arab Republic",
            "Taiwan",
            "Tajikistan",
            "Tanzania",
            "Thailand, Kingdom of",
            "Timor-Leste, Democratic Republic of",
            "Togo, Republic of",
            "Tonga",
            "Trinidad and Tobago",
            "Tunisia, Republic of",
            "Turkey, Republic of",
            "Turkmenistan",
            "Turks and Caicos Islands",
            "Tuvalu",
            "Uganda, Republic of",
            "Ukraine",
            "United Arab Emirates",
            "United States",
            "Uruguay",
            "Uzbekistan, Republic of",
            "Vanuatu",
            "Venezuela (Bolivarian Republic of)",
            "Vietnam, Socialist Republic of",
            "Yemen, Republic of",
            "Zambia, Republic of",
            "Zimbabwe, Republic of"
        };
    }
}