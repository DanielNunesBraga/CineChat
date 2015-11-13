namespace CineChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chat_options_not_null : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Chats", "title", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Chats", "title", c => c.String());
        }
    }
}
