﻿using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Context;
using Web.Model;

namespace Web.Repository
{
    public class NoteRepository : INoteRepository
    {
        private readonly NoteContext _context = null;

        public NoteRepository(IOptions<Settings> settings)
        {
            _context = new NoteContext(settings);
        }

        public async Task<IEnumerable<Note>> GetAllNotes()
        {
            try
            {
                return await _context.Notes
                        .Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<Note> GetNote(string id)
        {
            var filter = Builders<Note>.Filter.Eq("Id", id);

            try
            {
                return await _context.Notes
                                .Find(filter)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task AddNote(Note item)
        {
            try
            {
                await _context.Notes.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveNote(string id)
        {
            try
            {
                DeleteResult actionResult
                    = await _context.Notes.DeleteOneAsync(
                        Builders<Note>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateNote(string id, string body)
        {
            var filter = Builders<Note>.Filter.Eq(s => s.Id, id);
            var update = Builders<Note>.Update
                            .Set(s => s.Body, body)
                            .CurrentDate(s => s.UpdatedOn);

            try
            {
                UpdateResult actionResult
                    = await _context.Notes.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateNote(string id, Note item)
        {
            try
            {
                ReplaceOneResult actionResult
                    = await _context.Notes
                                    .ReplaceOneAsync(n => n.Id.Equals(id)
                                            , item
                                            , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveAllNotes()
        {
            try
            {
                DeleteResult actionResult
                    = await _context.Notes.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}
