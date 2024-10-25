﻿namespace Raggle.Abstractions.Memory;

/// <summary>
/// Represents a vector record.
/// </summary>
public class VectorPoint
{
    /// <summary>
    /// Gets or sets the vector ID.
    /// </summary>
    public required Guid VectorId { get; set; }

    /// <summary>
    /// Gets or sets the document ID.
    /// </summary>
    public required string DocumentId { get; set; }

    /// <summary>
    /// Text Content
    /// </summary>
    public required string Text { get; set; }

    /// <summary>
    /// Gets or sets the vector values.
    /// </summary>
    public float[] Vectors { get; set; } = [];

    /// <summary>
    /// Gets or sets the tags.
    /// </summary>
    public string[]? Tags { get; set; }
}
