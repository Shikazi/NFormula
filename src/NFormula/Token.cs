using System;
using System.Collections.Generic;

namespace NFormula
{
    public abstract class TokenMetadata
    {
    }

    public class LiteralValue : TokenMetadata
    {
        public object Value { get; }
        public LiteralValue(object value) => Value = value;
    }

    public class FunctionMetadata : TokenMetadata
    {
        public int ArgCount { get; set; }
        public FunctionMetadata(int argCount) => ArgCount = argCount;
    }

    public class Token
    {
        public TokenType Type { get; }
        public string Text { get; }

        private readonly Dictionary<Type, TokenMetadata> _metadataMap = new Dictionary<Type, TokenMetadata>();

        public Token(TokenType type, string text)
        {
            Type = type;
            Text = text;
        }

        public Token(TokenType type, string text, TokenMetadata metadata) : this(type, text)
        {
            if (metadata != null)
                _metadataMap[metadata.GetType()] = metadata;
        }

        // Lấy metadata theo kiểu T (nếu tồn tại)
        public T GetMetadata<T>() where T : TokenMetadata
        {
            if (_metadataMap.TryGetValue(typeof(T), out var value))
                return (T)value;
            return null;
        }

        // Đặt metadata trực tiếp
        public void SetMetadata<T>(T metadata) where T : TokenMetadata
        {
            _metadataMap[typeof(T)] = metadata;
        }

        // Đảm bảo metadata tồn tại, rồi truyền vào hàm xử lý
        public void EnsureMetadata<T>(Action<T> setAction) where T : TokenMetadata, new()
        {
            if (!_metadataMap.TryGetValue(typeof(T), out var existing))
            {
                existing = new T();
                _metadataMap[typeof(T)] = existing;
            }

            setAction((T)existing);
        }
    }
}