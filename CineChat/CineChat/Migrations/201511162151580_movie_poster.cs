namespace CineChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class movie_poster : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "poster", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "poster");
        }
    }
}
