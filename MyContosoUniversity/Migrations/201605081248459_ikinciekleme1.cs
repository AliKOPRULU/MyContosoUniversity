namespace MyContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ikinciekleme1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Student", name: "FirstMidName", newName: "Adı");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Student", name: "Adı", newName: "FirstMidName");
        }
    }
}
