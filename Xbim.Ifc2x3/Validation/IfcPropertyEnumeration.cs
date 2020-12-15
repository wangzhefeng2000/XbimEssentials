using System;
using Microsoft.Extensions.Logging;
using Xbim.Common;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Xbim.Common.Enumerations;
using Xbim.Common.ExpressValidation;
using Xbim.Ifc2x3.MeasureResource;
using Xbim.Ifc2x3.Interfaces;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProfileResource;
using Xbim.Ifc2x3.ProfilePropertyResource;
// ReSharper disable once CheckNamespace
// ReSharper disable InconsistentNaming
namespace Xbim.Ifc2x3.PropertyResource
{
	public partial class IfcPropertyEnumeration : IExpressValidatable
	{
		public enum IfcPropertyEnumerationClause
		{
			WR01,
		}

		/// <summary>
		/// Tests the express where-clause specified in param 'clause'
		/// </summary>
		/// <param name="clause">The express clause to test</param>
		/// <returns>true if the clause is satisfied.</returns>
		public bool ValidateClause(IfcPropertyEnumerationClause clause) {
			var retVal = false;
			try
			{
				switch (clause)
				{
					case IfcPropertyEnumerationClause.WR01:
						retVal = Functions.SIZEOF(this.EnumerationValues.Where(temp => !(Functions.TYPEOF(this.EnumerationValues.ItemAt(0)) == Functions.TYPEOF(temp)))) == 0;
						break;
				}
			} catch (Exception ex) {
				var log = Validation.ValidationLogging.CreateLogger<Xbim.Ifc2x3.PropertyResource.IfcPropertyEnumeration>();
				log?.LogError(string.Format("Exception thrown evaluating where-clause 'IfcPropertyEnumeration.{0}' for #{1}.", clause,EntityLabel), ex);
			}
			return retVal;
		}

		public virtual IEnumerable<ValidationResult> Validate()
		{
			if (!ValidateClause(IfcPropertyEnumerationClause.WR01))
				yield return new ValidationResult() { Item = this, IssueSource = "IfcPropertyEnumeration.WR01", IssueType = ValidationFlags.EntityWhereClauses };
		}
	}
}
