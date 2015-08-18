// Copyright 2007-2015 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.Saga.ConnectorFactories
{
    using System;
    using Connectors;
    using Pipeline.Filters;
    using Policies;


    public class InitiatedBySagaConnectorFactory<TSaga, TMessage> :
        ISagaConnectorFactory
        where TSaga : class, ISaga, InitiatedBy<TMessage>
        where TMessage : class, CorrelatedBy<Guid>
    {
        readonly InitiatedBySagaMessageFilter<TSaga, TMessage> _consumeFilter;
        readonly NewSagaPolicy<TSaga, TMessage> _policy;

        public InitiatedBySagaConnectorFactory()
        {
            _consumeFilter = new InitiatedBySagaMessageFilter<TSaga, TMessage>();

            ISagaFactory<TSaga, TMessage> sagaFactory = new DefaultSagaFactory<TSaga, TMessage>();

            _policy = new NewSagaPolicy<TSaga, TMessage>(sagaFactory, false);
        }

        public ISagaMessageConnector CreateMessageConnector()
        {
            return new CorrelatedSagaMessageConnector<TSaga, TMessage>(_consumeFilter, _policy, x => x.Message.CorrelationId);
        }
    }
}