﻿using Microsoft.Win32.SafeHandles;
using System;
using System.Buffers;
using System.Threading;

namespace MimeDetective.MemoryMapping;

// Inspired by:
// https://github.com/mgravell/Pipelines.Sockets.Unofficial/blob/main/src/Pipelines.Sockets.Unofficial/UnsafeMemory.cs

/// <summary>
///     A <see cref="System.Buffers.MemoryManager{T}" /> over a raw
///     <see cref="System.IO.MemoryMappedFiles.MemoryMappedViewAccessor" /> instance.
/// </summary>
internal sealed unsafe class UnsafeMemoryMappedViewMemory : MemoryManager<byte> {
    private readonly SafeMemoryMappedViewHandle _handle;
    private readonly int _length;
    private readonly byte* _pointer;
    private int _disposed;

    public UnsafeMemoryMappedViewMemory(SafeMemoryMappedViewHandle handle, int length) {
        _handle = handle;
        _length = length;
        _handle.AcquirePointer(ref _pointer);
    }

    public override Span<byte> GetSpan() {
        return new(_pointer, _length);
    }

    public override MemoryHandle Pin(int elementIndex = 0) {
        if (elementIndex < 0 || elementIndex >= _length) {
            throw new ArgumentOutOfRangeException(nameof(elementIndex));
        }

        return new(_pointer + elementIndex);
    }

    public override void Unpin() { }

    protected override void Dispose(bool disposing) {
        if (0 != Interlocked.Exchange(ref _disposed, 1)) {
            return;
        }

        _handle.ReleasePointer();
    }
}