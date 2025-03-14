/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using BH.oM.LifeCycleAssessment;
using BH.oM.LifeCycleAssessment.MaterialFragments;
using BH.oM.Base.Attributes;
using System.ComponentModel;
using System.Collections.Generic;

namespace BH.Engine.LifeCycleAssessment
{
    public static partial class Compute
    {
        /***************************************************/
        /****   Public Methods                          ****/
        /***************************************************/

        [Description("This is a simple calculation method for EPD QuantityTypes that are not yet fully supported. \n" +
            "This calculation is performed by multiplying the reference value by the selected field metric found within the EPD, divided by the QuantityTypeValue. \n" +
            "This method relies upon user input and is therefore at the discretion of the user to verify all results.")]
        [Input("referenceValue", "The amount, quantity, or value to evaluate against any Environmental Product Declaration.")]
        [Input("epd", "The Environmental Product Declaration to evaluate against the quantity.")]
        [Input("field", "The Environmental indicator to evaluate by. This value is queried from the EPD.")]
        [Input("phases", "Provide phases of life you wish to evaluate. Phases of life must be documented within EPDs for this method to work.")]
        [Input("exactMatch", "If true, the evaluation method will force an exact LCA phase match to solve for.")]
        [Output("result", "The total result of the desired metric based on the EnvironmentalProductDeclarationField.")]
        public static double EvaluateReferenceValue(double referenceValue, EnvironmentalProductDeclaration epd, EnvironmentalProductDeclarationField field, List<LifeCycleAssessmentPhases> phases, bool exactMatch = false)
        {
            if (epd == null)
            {
                BH.Engine.Base.Compute.RecordError("No EPD provided. Please provide a reference EPD.");
            }

            double epdValue = Query.GetEvaluationValue(epd, field, phases, exactMatch);

            if (referenceValue <= 0)
            {
                BH.Engine.Base.Compute.RecordError("No evaluation value was found within the EPD. Please try another.");
            }

            double qtValue = epd.QuantityTypeValue;

            string qt = System.Convert.ToString(Query.GetEPDQuantityType(epd));

            BH.Engine.Base.Compute.RecordNote($"Result is created by multiplying the ReferenceValue of {referenceValue} by the units of {qt} QuantityType extracted from " + epd.Name + " divided by {qtValue}.");

            double result = (referenceValue * epdValue) / qtValue;

            return result;
        }
        /***************************************************/

    }
}

