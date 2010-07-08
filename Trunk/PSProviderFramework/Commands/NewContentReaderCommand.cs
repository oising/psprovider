using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Text;

namespace PSProviderFramework.Commands
{
    [Cmdlet(VerbsCommon.New, "ContentReader")]
    public class NewContentReaderCommand : PSCmdlet
    {
        // more readable than Func<> or Action<> for powershell user
        public delegate object ReadHandler(long readCount);
        public delegate void SeekHandler(long offset, SeekOrigin origin);
        public delegate void CloseHandler();

        protected override void EndProcessing()
        {
            WriteObject(new ScriptContentReader(OnRead, OnSeek, OnClose));
        }

        [Parameter(Mandatory=true, Position=0)]
        public ReadHandler OnRead { get; set; }

        [Parameter(Mandatory = true, Position=1)]
        public SeekHandler OnSeek { get; set; }

        [Parameter(Mandatory = true, Position=2)]
        public CloseHandler OnClose { get; set; }

        public sealed class ScriptContentReader : IContentReader
        {
            private bool _closed;
            private readonly ReadHandler _reader;
            private readonly SeekHandler _seeker;
            private readonly CloseHandler _closer;

            public ScriptContentReader(ReadHandler reader, SeekHandler seeker, CloseHandler closer)
            {
                _reader = reader ?? delegate { return null; };
                _seeker = seeker ?? delegate { };
                _closer = closer ?? delegate { };
            }

            private void EnsureNotClosed()
            {
                if (_closed)
                {
                    throw new InvalidOperationException("Content Reader is closed/disposed!");
                }
            }

            public IList Read(long readCount)
            {
                EnsureNotClosed();
                
                object datum = _reader(readCount);

                // unwrap
                if (datum is PSObject)
                {
                    datum = ((PSObject) datum).BaseObject;
                }

                // if bound scriptblock returns an array with a single element
                // it will lose the containing array - need to add that back
                if (datum is IList)
                {
                    return datum as IList;
                }

                return new[] { datum };
            }

            public void Seek(long offset, SeekOrigin origin)
            {
                EnsureNotClosed();
                _seeker(offset, origin);
            }

            public void Close()
            {
                if (!_closed)
                {
                    _closer();
                    _closed = true;
                }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <filterpriority>2</filterpriority>
            public void Dispose()
            {
                Debug.WriteLine("ScriptContentReader.Dispose()");
                Close(); // _closer() ?
            }
        }
    }
}
