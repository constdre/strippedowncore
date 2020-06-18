using System;
using System.Threading.Tasks;
using ASPracticeCore.DAL;
using ASPracticeCore.Models;
using ASPracticeCore.Utils;
using Microsoft.EntityFrameworkCore;

class RepositoryShareable
{

    public async Task<string> UpdateSpecific(ApplicationContext context, Shareable update)
    {
        string status = Constants.SUCCESS +
        "_" + update.Title + " was updated successfully!";
        try
        {
            //retrieve record in db
            var shareable = await context.GetEntitySet<Shareable>().FirstOrDefaultAsync(s => s.Id == update.Id);
            //specific update data from client-side form
            shareable.Title = update.Title;
            shareable.Introduction = update.Introduction;
            shareable.Paragraphs = update.Paragraphs;
            await context.SaveChangesAsync();

        }
        catch (Exception)
        {
            status = Util.AttachStatusToMessage(Constants.FAILED, Constants.INTERNAL_ERROR);
        }
        return status;
    }

    public async Task<string> Update(ApplicationContext context, Shareable update)
    {
        string status = Constants.SUCCESS +
"_" + update.Title + " was updated successfully!";
        try
        {
            //retrieve record in db
            var shareable = await context.GetEntitySet<Shareable>().FindAsync(update.Id);
            context.Entry(shareable).CurrentValues.SetValues(shareable);
            
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            status = Util.AttachStatusToMessage(Constants.FAILED, Constants.INTERNAL_ERROR);
        }
        return status;
    }


}