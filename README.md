# DynamicPawnTableDef
helper library for PawnTables that have dynamic columns

# Usage
 - Use `DynamicPawnTable.DynamicPawnTableDef` instead of `PawnTableDef` in your def xml, the properties of the two are exactly the same. 
 - Call `Select( predicate Func<PawnColumnDef, bool> )` on your dynamic table def to filter the columns used.
 - Recreate the table, you'll need to do some reflection to access the private `table` field on your `MainTabWindow`.
 
# Example 
Defs/YourDefs.xml
```xml
<?xml version="1.0" encoding="utf-8"?>
<Defs>

  <DynamicPawnTable.DynamicPawnTableDef>
    <defName>Example</defName>
    <columns>
      <li>Label</li>
      <li>RemainingSpace</li>
    </columns>
  </DynamicPawnTable.DynamicPawnTableDef>

</Defs>
```

YourMainTabWindow.cs
```c#
public class MainTabWindow_Example : MainTabWindow_PawnTable
    {
        private static readonly FieldInfo _tableFieldInfo;

        static MainTabWindow_Example()
        {
            // set up the table field info to be used later
            _tableFieldInfo = typeof( MainTabWindow_PawnTable ).GetField( "table", BindingFlags.Instance | BindingFlags.NonPublic );
            
            // it's better to fail hard and early, in case the field got moved or renamed
            if ( _tableFieldInfo == null ){
                throw new NullReferenceException( "table field not found!" );
            }
        }
        
        public PawnTable Table
        {
            // simple property to make getting/setting the private field a bit easier
            get => _tableFieldInfo.GetValue( this ) as PawnTable;
            private set => _tableFieldInfo.SetValue( this, value );
        }
        
        private void RebuildTable()
        {
            var tableDef = DefDatabase<DynamicPawnTableDef>.GetNamed( "Example" );
            
            // predicate that allows only the columns you want to show
            var predicate = ( column ) => true;
            tableDef.Select( predicate );
            
            // pawngetter selects the pawns you want to show
            var pawngetter = () => Pawns
            
            // width and height of the pawntable
            int width = 500, height = 200;
            
            // rebuild the table. This also recalculates column widths, etc.
            Table = new PawnTable( tableDef, pawnGetter, width, height );
        }
    }
```
        
        
