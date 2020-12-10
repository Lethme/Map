using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Map.Hash
{
#nullable enable
    public class Hash : IEquatable<Hash>, IComparable<Hash>
    {
        public String Data { get; private set; } = String.Empty;
        public Hash() {  }
        public Hash(object? obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            this.Data = Encrypt(obj);
        }
        public void UpdateHash(object? obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            this.Data = Encrypt(obj);
        }
        public override bool Equals(object? obj)
        {
            return this.Data.Equals(Encrypt(obj));
        }
        public bool Equals(Hash? other)
        {
            return this.Data.Equals(other.Data);
        }
        public int CompareTo(Hash? other)
        {
            return this.Data.CompareTo(other.Data);
        }
        public override string ToString() => this.Data;
        public static bool operator ==(Hash hash, object obj) => hash.Equals(obj);
        public static bool operator ==(Hash left, Hash right) => left.Equals(right);
        public static bool operator !=(Hash hash, object obj) => !hash.Equals(obj);
        public static bool operator !=(Hash left, Hash right) => !left.Equals(right);
        public override int GetHashCode() => this.Data.GetHashCode();
        public static string Encrypt(object? obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            using var sha512 = SHA512.Create();
            var result = sha512.ComputeHash(Encoding.Default.GetBytes(JsonConvert.SerializeObject(obj)));
            return BitConverter.ToString(result).Replace("-", String.Empty).ToLower();
        }
        public static Hash Create() => new Hash();
        public static Hash Create(object? obj) => new Hash(obj);
    }
#nullable disable
}
