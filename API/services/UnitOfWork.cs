using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.interfaces;
using AutoMapper;

namespace API.services
{
    public class UnitOfWork : IUnitOfWork
    {
        /* This is the constructor of the class. It is used to initialize the class. */
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UnitOfWork(DataContext context = null, IMapper mapper = null)
        {
            _context = context;
            _mapper = mapper;
        }
        public IMessageRepository MessageRepository
        {
            get { return new MessageRepository(_context, _mapper); }
        }

        public IUserRepository UserRepository => new UserRepository(_context, _mapper);

        public IUserLikeRepository UserLikeRepository => new UserLikeRepository(_context, _mapper);

        public async Task<bool> CompleteAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}