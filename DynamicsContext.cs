using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using Polly;
using System;
using System.Configuration;
using System.Linq;

namespace CSharpCrudDynamics
{
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/mt634414.aspx
    /// </summary>
    public class DynamicsContext 
    {
        private static DynamicsContext instance;
        private CrmServiceClient client;

        private DynamicsContext()
        {
            try
            {
                var retry = 0;
                var policy = Policy
                    .Handle<Exception>()
                    .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(5), (exception, retryCount, context) =>
                    {
                        retry++;
                        instance = new DynamicsContext();
                        if (retry > 2)
                        {
                           //Log Error
                        }
                    });

                policy.Execute(() =>
                {
                    //TODO change the connection string
                    client = new CrmServiceClient(ConfigurationManager.ConnectionStrings["YOUR_CONNECTION_STRING"].ConnectionString);
                    if (!client.IsReady)
                    {
                        var message = $"Couldn't connect to Dynamics: {client.LastCrmError}";
                        
                    }
                });
            }
            catch (Exception ex)
            {
                //Log Error
            }
        }

        public static DynamicsContext GetInstance()
        {
            instance = new DynamicsContext();
            return instance;
        }

        public Guid Create(Entity entity)
        {
            var result = Guid.Empty;
            try
            {
                var requestForCreate = new ExecuteTransactionRequest()
                {
                    Requests = new OrganizationRequestCollection(),
                    ReturnResponses = true
                };

                var createRequest = new CreateRequest { Target = entity };
                requestForCreate.Requests.Add(createRequest);
                var bollException = false;
                var exceptionOut = new Exception();
                var retry = 0;
                var policy = Policy
                    .Handle<Exception>()
                    .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(5), (exception, retryCount, context) =>
                    {
                        retry++;
                        instance = new DynamicsContext();
                        if (retry > 2)
                        {
                            exceptionOut = exception;
                            bollException = true;
                        }
                    });

                if (bollException)
                {
                   //Log Error
                }

                policy.Execute(() =>
                {
                    var responseForCreate = (ExecuteTransactionResponse)client.Execute(requestForCreate);
                    foreach (var response in responseForCreate.Responses)
                        Guid.TryParse(response.Results["id"].ToString(), out result);
                });
            }
            catch (Exception ex)
            {
                //Log Error
            }
            return result;
        }

        public void Update(Entity entity)
        {
            var requestForUpdates = new ExecuteTransactionRequest()
            {
                Requests = new OrganizationRequestCollection(),
                ReturnResponses = true
            };
            try
            {

                var updateRequest = new UpdateRequest { Target = entity };
                requestForUpdates.Requests.Add(updateRequest);

                var retry = 0;
                var policy = Policy
                    .Handle<Exception>()
                    .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(5), (exception, retryCount, context) =>
                    {
                        retry++;
                        instance = new DynamicsContext();
                        if (retry > 2)
                        {
                            //Log Error
                        }
                    });

                policy.Execute(() =>
                {
                    var responseForUpdates = (ExecuteTransactionResponse)client.Execute(requestForUpdates);
                });
            }
            catch (Exception ex)
            {
                //Log Error
            }
        }

        public OrganizationResponse Execute(OrganizationRequest request)
        {
            var organizationResponse = new OrganizationResponse();

            try
            {
                var retry = 0;
                var policy = Policy
                    .Handle<Exception>()
                    .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(5), (exception, retryCount, context) =>
                    {
                        retry++;
                        instance = new DynamicsContext();
                        if (retry > 2)
                        {
                            //Log Error
                        }
                    });

                policy.Execute(() =>
                {
                    organizationResponse = client.Execute(request);
                });
            }
            catch (Exception ex)
            {
                //Log Error
            }
            return organizationResponse;
        }

        public EntityCollection RetrieveMultipleFetch(string fetch)
        {
            var entityCollection = new EntityCollection();
            try
            {
                var retry = 0;
                var policy = Policy
                    .Handle<Exception>()
                    .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(5), (exception, retryCount, context) =>
                    {
                        retry++;
                        instance = new DynamicsContext();
                        if (retry > 2)
                        {
                            //Log Error
                        }
                    });

                policy.Execute(() =>
                {
                    entityCollection = client.RetrieveMultiple(new FetchExpression(fetch));
                });
            }
            catch (Exception ex)
            {
                //Log Error
            }
            return entityCollection;
        }

        public IQueryable<Entity> CreateQuery(string entity)
        {
            IQueryable<Entity> result = null;
            try
            {
                OrganizationServiceContext context = new OrganizationServiceContext(client);
                result = context.CreateQuery(entity);
            }
            catch (Exception exception)
            {
                //Log Error
            }
            return result;
        }

        public EntityCollection RetrieveMultipleQueryExpression(QueryExpression queryExpression)
        {

            var entityCollection = new EntityCollection();
            try
            {
                var retry = 0;
                var policy = Policy
                    .Handle<Exception>()
                    .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(5), (exception, retryCount, context) =>
                    {
                        retry++;
                        instance = new DynamicsContext();
                        if (retry > 2)
                        {
                            //Log Error
                        }
                    });

                policy.Execute(() =>
                {
                    if (client.IsReady)
                        entityCollection = client.RetrieveMultiple(queryExpression);
                });
            }
            catch (Exception ex)
            {
                //Log Error
            }
            return entityCollection;
        }

        public Entity Retrieve(string entityName, Guid id, ColumnSet columnSet)
        {
            var entity = new Entity();
            try
            {

                var retry = 0;
                var policy = Policy
                    .Handle<Exception>()
                    .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(5), (exception, retryCount, context) =>
                    {
                        retry++;
                        instance = new DynamicsContext();
                        if (retry > 2)
                        {
                            //Log Error
                        }
                    });

                policy.Execute(() =>
                {
                    entity = client.Retrieve(entityName, id, columnSet);
                });
            }
            catch (Exception ex)
            {
                //Log Error
            }
            return entity;
        }

        public void SetState(Entity entity, int stateCode, int statusCode)
        {
            try
            {
                entity["statecode"] = new OptionSetValue(stateCode);
                entity["statuscode"] = new OptionSetValue(statusCode);
                Update(entity);
            }
            catch (Exception ex)
            {
                //Log Error
            }
        }
    }
}