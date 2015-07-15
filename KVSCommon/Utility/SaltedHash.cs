﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace KVSCommon.Utility
{
    public class SaltedHash
    {
        /// <summary>
        /// The constructor takes a HashAlgorithm as a parameter.
        /// </summary>
        /// <param name="hashAlgorithm">
        /// A <see cref="hashAlgorithm"/> HashAlgorihm which is derived from HashAlgorithm. C# provides
        /// the following classes: SHA1Managed,SHA256Managed, SHA384Managed, SHA512Managed and MD5CryptoServiceProvider
        /// </param>
        /// <param name="theSaltLength">Length of the generated Salt.</param>
        public SaltedHash(HashAlgorithm hashAlgorithm, int theSaltLength)
        {
            this.HashProvider = hashAlgorithm;
            this.SalthLength = theSaltLength;
        }

        /// <summary>
        /// Default constructor which initialises the SaltedHash with the SHA256Managed algorithm
        /// and a Salt of 4 bytes ( or 4*8 = 32 bits)
        /// </summary>
        public SaltedHash()
            : this(new SHA256Managed(), 4)
        {
        }

        /// <summary>
        /// Der zu benutzende HashProvider.
        /// </summary>
        private HashAlgorithm HashProvider
        {
            get;
            set;
        }

        /// <summary>
        /// Länge des SaltString.
        /// </summary>
        private int SalthLength
        {
            get;
            set;
        }

        /// <summary>
        /// Given a data block this routine returns both a Hash and a Salt
        /// </summary>
        /// <param name="data">
        /// A <see cref="System.Byte"/>byte array containing the data from which to derive the salt
        /// </param>
        /// <param name="hash">
        /// A <see cref="System.Byte"/>byte array which will contain the hash calculated
        /// </param>
        /// <param name="salt">
        /// A <see cref="System.Byte"/>byte array which will contain the salt generated
        /// </param>
        public void GetHashAndSalt(byte[] data, out byte[] hash, out byte[] salt)
        {
            // Allocate memory for the salt
            salt = new byte[this.SalthLength];

            // Strong runtime pseudo-random number generator, on Windows uses CryptAPI
            // on Unix /dev/urandom
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();

            // Create a random salt
            random.GetNonZeroBytes(salt);

            // Compute hash value of our data with the salt.
            hash = this.ComputeHash(data, salt);
        }

        /// <summary>
        /// The routine provides a wrapper around the GetHashAndSalt function providing conversion
        /// from the required byte arrays to strings. Both the Hash and Salt are returned as Base-64 encoded strings.
        /// </summary>
        /// <param name="data">
        /// A <see cref="System.String"/> string containing the data to hash
        /// </param>
        /// <param name="hash">
        /// A <see cref="System.String"/> base64 encoded string containing the generated hash
        /// </param>
        /// <param name="salt">
        /// A <see cref="System.String"/> base64 encoded string containing the generated salt
        /// </param>
        public void GetHashAndSaltString(string data, out string hash, out string salt)
        {
            byte[] hashOut;
            byte[] saltOut;

            // Obtain the Hash and Salt for the given string
            this.GetHashAndSalt(Encoding.UTF8.GetBytes(data), out hashOut, out saltOut);

            // Transform the byte[] to Base-64 encoded strings
            hash = Convert.ToBase64String(hashOut);
            salt = Convert.ToBase64String(saltOut);
        }

        /// <summary>
        /// This routine verifies whether the data generates the same hash as we had stored previously
        /// </summary>
        /// <param name="data">The data to verify </param>
        /// <param name="hash">The hash we had stored previously</param>
        /// <param name="salt">The salt we had stored previously</param>
        /// <returns>True on a succesfull match</returns>
        public bool VerifyHash(byte[] data, byte[] hash, byte[] salt)
        {
            byte[] newHash = this.ComputeHash(data, salt);

            // No easy array comparison in C# -- we do the legwork
            if (newHash.Length != hash.Length)
            {
                return false;
            }

            for (int lp = 0; lp < hash.Length; lp++)
            {
                if (!hash[lp].Equals(newHash[lp]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// This routine provides a wrapper around VerifyHash converting the strings containing the
        /// data, hash and salt into byte arrays before calling VerifyHash.
        /// </summary>
        /// <param name="data">A UTF-8 encoded string containing the data to verify</param>
        /// <param name="hash">A base-64 encoded string containing the previously stored hash</param>
        /// <param name="salt">A base-64 encoded string containing the previously stored salt</param>
        /// <returns>True, if verified, else false.</returns>
        public bool VerifyHashString(string data, string hash, string salt)
        {
            byte[] hashToVerify = Convert.FromBase64String(hash);
            byte[] saltToVerify = Convert.FromBase64String(salt);
            byte[] dataToVerify = Encoding.UTF8.GetBytes(data);
            return this.VerifyHash(dataToVerify, hashToVerify, saltToVerify);
        }

        /// <summary>
        /// The actual hash calculation is shared by both GetHashAndSalt and the VerifyHash functions
        /// </summary>
        /// <param name="data">A byte array of the Data to Hash</param>
        /// <param name="salt">A byte array of the Salt to add to the Hash</param>
        /// <returns>A byte array with the calculated hash</returns>
        private byte[] ComputeHash(byte[] data, byte[] salt)
        {
            // Allocate memory to store both the Data and Salt together
            byte[] dataAndSalt = new byte[data.Length + this.SalthLength];

            // Copy both the data and salt into the new array
            Array.Copy(data, dataAndSalt, data.Length);
            Array.Copy(salt, 0, dataAndSalt, data.Length, this.SalthLength);

            // Calculate the hash
            // Compute hash value of our plain text with appended salt.
            return this.HashProvider.ComputeHash(dataAndSalt);
        }
    }
}
