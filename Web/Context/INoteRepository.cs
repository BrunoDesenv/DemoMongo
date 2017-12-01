using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Context
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllNotes();
        Task<Note> GetNote(string id);

        // add new note document
        Task AddNote(Note item);

        // remove a single document / note
        Task<bool> RemoveNote(string id);

        // update just a single document / note
        Task<bool> UpdateNote(string id, string body);

        // should be used with high cautious, only in relation with demo setup
        Task<bool> RemoveAllNotes();
    }
}
