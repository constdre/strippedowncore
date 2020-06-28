using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPracticeCore.DAL;
using ASPracticeCore.Models;
using ASPracticeCore.Repositories;
using ASPracticeCore.Utils;
using Microsoft.EntityFrameworkCore;

class RepositoryShareable
{

    public async Task UpdateExcludeImages(ApplicationContext context, Shareable update)
    {
        //Specific update - modified props only

        string status = Constants.SUCCESS + "_" + update.Title + " was updated successfully!";


        //retrieve record in db
        var shareable = await context.GetEntitySet<Shareable>().FirstOrDefaultAsync(s => s.Id == update.Id);

        //update only specific props from the form data
        shareable.Title = update.Title;
        shareable.Introduction = update.Introduction;
        shareable.Paragraphs = update.Paragraphs;//updates content or adds new paragraph

        await context.SaveChangesAsync();



        //Delete paragraphs removed in the form.
        //Check which ids are no longer present in the updated list
        var updatedParags = update.Paragraphs;

        //get original list of paragraphs
        var origParags = context.GetEntitySet<Paragraph>().
                Where(p => p.ShareableId == shareable.Id);

        //hashset for quicker searching
        var updatedParIds = new HashSet<int>(updatedParags.Select(p => p.Id));//to just compare ids

        //check if a paragraph is removed in the updated list, delete
        await origParags.ForEachAsync(origParag =>
        {
            if (!updatedParIds.Contains(origParag.Id))
            {
                context.Remove(origParag);
            }
        });

        await context.SaveChangesAsync();
    }


    public async Task Update(ApplicationContext context, Shareable update)
    {
        //Basic update of non-reference type props
        var shareable = await context.GetEntitySet<Shareable>().FindAsync(update.Id);
        context.Entry(shareable).CurrentValues.SetValues(shareable);//does not set object-type properties
        await context.SaveChangesAsync();

    }


}