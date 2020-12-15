using System;
using Microsoft.Extensions.Logging;
using Xbim.Common;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Xbim.Common.Enumerations;
using Xbim.Common.ExpressValidation;
using Xbim.Ifc4.Interfaces;
// ReSharper disable once CheckNamespace
// ReSharper disable InconsistentNaming
namespace Xbim.Ifc4.ProfileResource
{
	public partial class IfcRoundedRectangleProfileDef : IExpressValidatable
	{
		public enum IfcRoundedRectangleProfileDefClause
		{
			ValidRadius,
		}

		/// <summary>
		/// Tests the express where-clause specified in param 'clause'
		/// </summary>
		/// <param name="clause">The express clause to test</param>
		/// <returns>true if the clause is satisfied.</returns>
		public bool ValidateClause(IfcRoundedRectangleProfileDefClause clause) {
			var retVal = false;
			try
			{
				switch (clause)
				{
					case IfcRoundedRectangleProfileDefClause.ValidRadius:
						retVal = ((RoundingRadius <= (this/* as IfcRectangleProfileDef*/.XDim / 2)) && (RoundingRadius <= (this/* as IfcRectangleProfileDef*/.YDim / 2)));
						break;
				}
			} catch (Exception  ex) {
				var log = Validation.ValidationLogging.CreateLogger<Xbim.Ifc4.ProfileResource.IfcRoundedRectangleProfileDef>();
				log?.LogError(string.Format("Exception thrown evaluating where-clause 'IfcRoundedRectangleProfileDef.{0}' for #{1}.", clause,EntityLabel), ex);
			}
			return retVal;
		}

		public virtual IEnumerable<ValidationResult> Validate()
		{
			if (!ValidateClause(IfcRoundedRectangleProfileDefClause.ValidRadius))
				yield return new ValidationResult() { Item = this, IssueSource = "IfcRoundedRectangleProfileDef.ValidRadius", IssueType = ValidationFlags.EntityWhereClauses };
		}
	}
}
