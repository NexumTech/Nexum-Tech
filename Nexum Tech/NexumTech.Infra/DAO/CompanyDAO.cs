﻿using Dapper;
using NexumTech.Infra.DAO.Interfaces;
using NexumTech.Infra.Models;
using System.Text;

namespace NexumTech.Infra.DAO
{
    public class CompanyDAO : ICompanyDAO
    {
        private readonly BaseDatabaseService _baseDatabaseService;

        public CompanyDAO(BaseDatabaseService baseDatabaseService)
        {
            _baseDatabaseService = baseDatabaseService;
        }

        public async Task<bool> CreateCompany(CompanyViewModel companyViewModel)
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    string sql = @"INSERT INTO tb_company
                                        ([OwnerId]
                                        ,[Name]
                                        ,[Description]
                                        ,[Logo])
                                    VALUES
                                        (@OwnerId
                                        ,@Name
                                        ,@Description
                                        ,@Logo)";

                    await connection.QueryAsync(sql, new
                    {
                        @OwnerId = companyViewModel.OwnerId,
                        @Name = companyViewModel.Name.Trim(),
                        @Description = companyViewModel.Description.Trim(),
                        @Logo = companyViewModel.Logo,
                    });

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CompanyViewModel> GetCompany(int ownerId)
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    string sql = "SELECT * FROM tb_company WHERE OwnerId = @OwnerId";

                    var company = await connection.QueryFirstOrDefaultAsync<CompanyViewModel>(sql, new
                    {
                        @OwnerId = ownerId,
                    });

                    return company;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CompanyViewModel> GetCompany(string name)
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    string sql = "SELECT * FROM tb_company WHERE Name = @Name";

                    var company = await connection.QueryFirstOrDefaultAsync<CompanyViewModel>(sql, new
                    {
                        @Name = name.Trim(),
                    });

                    return company;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateCompany(CompanyViewModel companyViewModel)
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    StringBuilder sql = new StringBuilder();

                    sql.Append("UPDATE tb_company SET Name = @Name, Description = @Description");

                    if (companyViewModel.Logo.Length > 0)
                    {
                        sql.Append(", Logo = @Logo");
                    }

                    sql.Append(" WHERE OwnerId = @OwnerId");

                    await connection.QueryAsync<int>(sql.ToString(), new
                    {
                        @OwnerId = companyViewModel.OwnerId,
                        @Logo = companyViewModel.Logo,
                        @Name = companyViewModel.Name.Trim(),
                        @Description = companyViewModel.Description.Trim(),
                    });

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteCompany(int ownerId)
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    string sql = "DELETE FROM tb_company WHERE OwnerId = @OwnerId";

                    var company = await connection.QueryAsync<int>(sql, new
                    {
                        @OwnerId = ownerId,
                    });

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}