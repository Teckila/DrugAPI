using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrugAPI.Models
{
    public class Drug
    {

        /// <summary>
        /// Code of the Drug
        /// </summary>
        [Key]
        public string Code { get; set; }
        /// <summary>
        ///Label of the drug
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// Description of the drug
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Price of the drug
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Check if drug definition is right, else set errorMessage to indicate why
        /// </summary>
        /// <param name="drug">drug to test</param>
        /// <param name="errorMessage">Error message to indicate why it's incorrect</param>
        /// <returns></returns>
        public static bool isCorrectDrug(Drug drug, out string errorMessage)
        {
            bool isDrugCorrect = true;
            errorMessage = "";

            if (drug.Code.Length > 30)
            {
                errorMessage = $"Error Occured : We don't accept code that is more than 30 characters";
                isDrugCorrect = false;
            }

            if (drug.Label.Length > 100)
            {
                errorMessage = $"Error Occured : We don't accept label that is more than 100 characters";
                isDrugCorrect = false;
            }

            if (drug.Price <= 0)
            {
                errorMessage = $"Error Occured : We don't accept price that is less or equal to 0";
                isDrugCorrect = false;
            }

            return isDrugCorrect;
        }
    }
}
