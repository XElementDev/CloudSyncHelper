﻿using NamedPipeWrapper;
using System;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using XElement.CloudSyncHelper.Logic.Execution.MkLink;
using XElement.DotNet.System.Environment.UserInformation;

namespace XElement.CloudSyncHelper.UI.Win32.LinkCreator.Logic
{
#region not unit-tested
    public class Server
    {
        public Server( string pipeName )
        {
            var mkLinkExecutorDependencies = new DependenciesDTO
            {
                RoleInfoRetriever = new WindowsPrincipalRetriever() // TODO: Use MEF
            };
            var mkLinkExecutorFactory = new Factory( mkLinkExecutorDependencies );
            this._executorFactory = mkLinkExecutorFactory;
            this._pipeName = pipeName;

            this._syncRoot = new object();
            this._waitHandle = new ManualResetEvent( false );

            InitializeServerPipe();

            return;
        }


        private void ClientConnected( NamedPipeConnection<string, string> connection )
        {
            Logger.Get().Log( "A client connected to the server." );
            return;
        }


        private void ClientDisconnected( NamedPipeConnection<string, string> connection )
        {
            Logger.Get().Log( "A client disconnected from the server." );
            return;
        }


        private void ClientMessage( NamedPipeConnection<string, string> connection, string message )
        {
            var logMessage = $"Received the following message from client: {message}";
            Logger.Get().Log( logMessage );

            lock ( this._syncRoot )
            {
                this._clientMessage = message;
                this._waitHandle.Set();
            }

            return;
        }


        private static PipeSecurity CreatePipeSecurity()
        {
            //  --> 2016-08-11, Ian: 
            //      based on: https://stackoverflow.com/questions/13174660/namedpipeclientstream-can-not-access-to-namedpipeserverstream-under-session-0
            var pipeSecurity = new PipeSecurity();
            var sid = new SecurityIdentifier( WellKnownSidType.WorldSid, null );
            var accessRule = new PipeAccessRule( sid, 
                                                 PipeAccessRights.ReadWrite, 
                                                 AccessControlType.Allow );
            pipeSecurity.AddAccessRule( accessRule );
            return pipeSecurity;
        }


        private void DoWork( string message )
        {
            var parameters = new MessageParser().Parse( message );

            if ( parameters == null )
            {
                var error = String.Format( "Parsed message (of type {0}) must not be null.",
                                           typeof( ParametersDTO ).ToString() );
                throw new InvalidOperationException( error );
            }
            else
            {
                var parametersAsString = Logger.Get().GetLogRepresentationOf( parameters );
                Logger.Get().Log( $"Parsed message looks like this: {parametersAsString}" );

                var executor = this._executorFactory.Get( parameters );
                executor.Execute();
            }

            return;
        }


        private void InitializeServerPipe()
        {
            PipeSecurity pipeSecurity = Server.CreatePipeSecurity();
            this._serverPipe = new NamedPipeServer<string>( this._pipeName, pipeSecurity );
            this.SubscribeEvents();
            return;
        }


        private void OnError( Exception exception )
        {
            throw exception;
        }


        public void Start()
        {
            this._serverPipe.Start();
            Logger.Get().Log( "Server started." );
            return;
        }


        public void StayAlive()
        {
            // ↓    based on https://stackoverflow.com/questions/8881396/cpu-usage-increasing-up-to-100-in-infinite-loop-in-thread
            while ( true )
            {
                this._waitHandle.WaitOne();

                lock ( this._syncRoot )
                {
                    this.DoWork( this._clientMessage );
                    this._serverPipe.PushMessage( String.Empty );

                    this._waitHandle.Reset();
                }
            }
        }


        private void SubscribeEvents()
        {
            this._serverPipe.ClientConnected += this.ClientConnected;
            this._serverPipe.ClientDisconnected += this.ClientDisconnected;
            this._serverPipe.ClientMessage += this.ClientMessage;
            this._serverPipe.Error += this.OnError;
            return;
        }


        private string _clientMessage;

        private IFactory _executorFactory;

        private string _pipeName;

        private NamedPipeServer<string> _serverPipe;

        private object _syncRoot;

        private ManualResetEvent _waitHandle;
    }
#endregion
}
