using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace DynamicPawnTable
{
    public class DynamicPawnTableDef: PawnTableDef
    {
        private List<PawnColumnDef> intColumns;

        public List<PawnColumnDef> AllColumnsListForReading => intColumns.NullOrEmpty()
            ? new List<PawnColumnDef>()
            : new List<PawnColumnDef>( intColumns );

        public void Select( Func<PawnColumnDef, bool> validator )
        {
            columns = intColumns.Where( validator ).ToList();
        }

        public override void PostLoad()
        {
            base.PostLoad();
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                intColumns = new List<PawnColumnDef>( columns );
            });
        }
    }
}