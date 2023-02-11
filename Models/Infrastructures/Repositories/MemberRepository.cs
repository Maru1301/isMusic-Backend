﻿using api.iSMusic.Models.DTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.Infrastructures.Repositories
{
	public class MemberRepository: IRepository, IMemberRepository
	{
		private readonly AppDbContext _db;

		public MemberRepository(AppDbContext db)
		{
			_db = db;
		}

		public Member? GetMember(int memberId)
		{
			return _db.Members.SingleOrDefault(m => m.Id == memberId);
		}

		public async Task<Member?> GetMemberAsync(int memberId)
		{
			return await _db.Members.SingleOrDefaultAsync(m => m.Id == memberId);
		}
	}
}