//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace C3PO.Data.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class vessel
    {
        public vessel()
        {
            this.boardings = new HashSet<boarding>();
        }
    
        public string name { get; set; }
        public string callsign { get; set; }
        public decimal vid { get; set; }
        public string mmsi { get; set; }
        public string imo { get; set; }
        public string sconum { get; set; }
        public Nullable<float> latitude { get; set; }
        public Nullable<float> longitude { get; set; }
        public Nullable<int> typeid { get; set; }
        public Nullable<float> length { get; set; }
        public string hullcolor { get; set; }
        public string funnelcolor { get; set; }
        public string superstructurecolor { get; set; }
        public Nullable<int> oid { get; set; }
        public Nullable<int> tid { get; set; }
        public Nullable<bool> ispublic { get; set; }
        public string registrationnumber { get; set; }
        public string externalmarkings { get; set; }
        public string inmarsat { get; set; }
        public string fueloilstatus { get; set; }
        public string dailyfueloilconsumption { get; set; }
        public string fuelloadedcost { get; set; }
        public string plantstatus { get; set; }
        public Nullable<float> beam { get; set; }
        public Nullable<float> draft { get; set; }
        public bool islengthestimated { get; set; }
        public bool isbeamestimated { get; set; }
        public bool isdraftestimated { get; set; }
        public string lastportofcall { get; set; }
        public Nullable<bool> hassatphones { get; set; }
        public Nullable<bool> hascellphones { get; set; }
        public Nullable<int> crewtotal { get; set; }
        public Nullable<int> skiffs { get; set; }
        public string portofregistry { get; set; }
        public string nextportofcall { get; set; }
        public string pilothousedescription { get; set; }
        public Nullable<int> flagcid { get; set; }
        public Nullable<int> lastportofcallcid { get; set; }
        public Nullable<int> registrycid { get; set; }
        public Nullable<int> nextportofcallcid { get; set; }
        public Nullable<bool> securityteamaboard { get; set; }
        public Nullable<int> securityteamsize { get; set; }
        public Nullable<bool> securityteampartofcrew { get; set; }
        public Nullable<int> fueltypeid { get; set; }
        public string hiddencompartments { get; set; }
        public string explosivesdetected { get; set; }
        public string normaltradingroute { get; set; }
        public Nullable<bool> aisrequired { get; set; }
        public Nullable<bool> aisonboard { get; set; }
        public Nullable<bool> aisfunctional { get; set; }
        public Nullable<bool> aiscorrect { get; set; }
        public Nullable<decimal> etayear { get; set; }
        public Nullable<decimal> etamonth { get; set; }
        public Nullable<decimal> etaday { get; set; }
        public Nullable<decimal> lastportofcallyear { get; set; }
        public Nullable<decimal> lastportofcallmonth { get; set; }
        public Nullable<decimal> lastportofcallday { get; set; }
        public Nullable<bool> aisison { get; set; }
        public string condition { get; set; }
        public string capabilities { get; set; }
        public string material { get; set; }
        public string conditionnotes { get; set; }
        public string report { get; set; }
    
        public virtual ICollection<boarding> boardings { get; set; }
    }
}
