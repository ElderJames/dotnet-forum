﻿using LambdaForums.Data;
using LambdaForums.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Service
{
    public class ForumService : IForum
    {

        private readonly ApplicationDbContext _context;
        private readonly IPost _postService;

        public ForumService(ApplicationDbContext context, IPost postService)
        {
            _context = context;
            _postService = postService;
        }

        public Task Add(Forum forum)
        {
            throw new NotImplementedException();
        }

        public async Task Create(Data.Models.Forum forum)
        {
            _context.Add(forum);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var forum = GetById(id);
            _context.Remove(forum);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<ApplicationUser> GetActiveUsers(int forumId)
        {
            var posts = GetById(forumId).Posts;

            if (posts == null || !posts.Any())
            {
                return new List<ApplicationUser>();
            }

            return _postService.GetAllUsers(posts);
        }

        public IEnumerable<Forum> GetAll()
        {
            return _context.Forums.Include(forum => forum.Posts);
        }


        public IEnumerable<ApplicationUser> GetAllActiveUsers(int forumId)
        {
            throw new NotImplementedException();
        }

        public Forum GetById(int id)
        {
            var forum = _context.Forums.Where(f => f.Id == id)
                .Include(f => f.Posts).ThenInclude(p => p.User)
                .Include(f => f.Posts).ThenInclude(p => p.Replies).ThenInclude(r => r.User)
                .FirstOrDefault();

            return forum;
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            return _postService.GetFilteredPosts(searchQuery);
        }

        public IEnumerable<Post> GetFilteredPosts(int forumId, string searchQuery)
        {
            if (forumId == 0) return _postService.GetFilteredPosts(searchQuery);

            var forum = GetById(forumId);

            return string.IsNullOrEmpty(searchQuery)
                ? forum.Posts
                : forum.Posts.Where(post
                    => post.Title.Contains(searchQuery) || post.Content.Contains(searchQuery));
        }


        public Post GetLatestPost(int forumId)
        {
            throw new NotImplementedException();
        }

        public Task SetForumImage(int id, Uri uri)
        {
            throw new NotImplementedException();
        }

        public Task UpdateForumDescription(int id, string description)
        {
            throw new NotImplementedException();
        }

        public Task UpdateforumTitle(int forumId, string newDescription)
        {
            throw new NotImplementedException();
        }

        public Task UpdateForumTitle(int forumId, string newDescription)
        {
            throw new NotImplementedException();
        }
    }
}
