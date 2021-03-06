namespace RTCV.Launcher
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    internal static class RTC_Extensions
    {
        public static IEnumerable<T> OrderByNatural<T>(this IEnumerable<T> items, Func<T, string> selector, StringComparer stringComparer = null)
        {
            var regex = new Regex(@"\d+", RegexOptions.Compiled);

            var maxDigits = items
                                .SelectMany(i => regex.Matches(selector(i)).Cast<Match>().Select(digitChunk => (int?)digitChunk.Value.Length))
                                .Max() ?? 0;

            return items.OrderBy(i => regex.Replace(selector(i), match => match.Value.PadLeft(maxDigits, '0')), stringComparer ?? StringComparer.CurrentCulture);
        }
        public static IEnumerable<T> OrderByNaturalDescending<T>(this IEnumerable<T> items, Func<T, string> selector, StringComparer stringComparer = null)
        {
            var regex = new Regex(@"\d+", RegexOptions.Compiled);

            var maxDigits = items
                                .SelectMany(i => regex.Matches(selector(i)).Cast<Match>().Select(digitChunk => (int?)digitChunk.Value.Length))
                                .Max() ?? 0;

            return items.OrderBy(i => regex.Replace(selector(i), match => match.Value.PadLeft(maxDigits, '0')), stringComparer ?? StringComparer.CurrentCulture).Reverse();
        }

        public static DialogResult getInputBox(string title, string promptText, ref string value)
        {
            var form = new Form();
            var label = new Label();
            var textBox = new TextBox();
            var buttonOk = new Button();
            var buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor |= AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        #region ARRAY EXTENSIONS
        public static T[] SubArray<T>(this T[] data, long index, long length)
        {
            var result = new T[length];

            if (data == null)
            {
                return null;
            }

            Array.Copy(data, index, result, 0, length);
            return result;
        }
        #endregion

        #region STRING EXTENSIONS
        public static string ToBase64(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        public static string FromBase64(this string base64)
        {
            var data = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(data);
        }

        internal static byte[] GetBytes(this string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        #endregion

        #region COLOR EXTENSIONS

        /// <summary>
        /// Creates color with corrected brightness.
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1.
        /// Negative values produce darker colors.</param>
        /// <returns>
        /// Corrected <see cref="Color"/> structure.
        /// </returns>
        public static Color ChangeColorBrightness(this Color color, float correctionFactor)
        {
            var red = (float)color.R;
            var green = (float)color.G;
            var blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

        #endregion

        #region CONTROL EXTENSIONS

        internal static List<Control> getControlsWithTag(this Control.ControlCollection controls)
        {
            var allControls = new List<Control>();

            foreach (Control c in controls)
            {
                if (c.Tag != null)
                {
                    allControls.Add(c);
                }

                if (c.HasChildren)
                {
                    allControls.AddRange(c.Controls.getControlsWithTag()); //Recursively check all children controls as well; ie groupboxes or tabpages
                }
            }

            return allControls;
        }

        #endregion
        internal static void RecursiveCopyNukeReadOnly(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                RecursiveCopyNukeReadOnly(dir, target.CreateSubdirectory(dir.Name));
            }

            if (!target.Exists)
            {
                target.Create();
            }

            foreach (FileInfo file in source.GetFiles())
            {
                try
                {
                    file.CopyTo(Path.Combine(target.FullName, file.Name), true);
                }
                catch (Exception ex)
                {
                    File.SetAttributes(file.FullName, FileAttributes.Normal);
                    file.CopyTo(Path.Combine(target.FullName, file.Name), true);

                    _ = ex;
                }
            }
        }
        public static List<string> RecursiveDeleteNukeReadOnly(string path)
        {
            return RecursiveDeleteNukeReadOnly(new DirectoryInfo(path));
        }
        internal static List<string> RecursiveDeleteNukeReadOnly(DirectoryInfo target)
        {
            var failedList = new List<string>();
            foreach (DirectoryInfo dir in target.GetDirectories())
            {
                failedList.AddRange(RecursiveDeleteNukeReadOnly(dir));
            }
            foreach (FileInfo file in target.GetFiles())
            {
                try
                {
                    File.Delete(file.FullName);
                }
                catch (Exception ex)
                {
                    try
                    {
                        File.SetAttributes(file.FullName, FileAttributes.Normal);
                        File.Delete(file.FullName);
                    }
                    catch (Exception ex2)
                    {
                        failedList.Add(file.FullName);
                        _ = ex2;
                    }
                    _ = ex;
                }
            }
            if (target.GetFiles().Length == 0)
            {
                target.Delete();
            }

            return failedList;
        }
    }

    // Used code from this https://github.com/wasabii/Cogito/blob/master/Cogito.Core/RandomExtensions.cs
    // MIT Licensed. thank you very much.
    static class RandomExtensions
    {
        public static long RandomLong(this Random rnd)
        {
            var buffer = new byte[8];
            rnd.NextBytes(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }

        public static long RandomLong(this Random rnd, long min, long max)
        {
            EnsureMinLEQMax(ref min, ref max);
            var numbersInRange = unchecked(max - min + 1);
            if (numbersInRange < 0)
            {
                throw new ArgumentException("Size of range between min and max must be less than or equal to Int64.MaxValue");
            }

            var randomOffset = RandomLong(rnd);
            if (IsModuloBiased(randomOffset, numbersInRange))
            {
                return RandomLong(rnd, min, max); // Try again
            }
            else
            {
                return min + PositiveModuloOrZero(randomOffset, numbersInRange);
            }
        }

        public static long RandomLong(this Random rnd, long max)
        {
            return rnd.RandomLong(0, max);
        }

        static bool IsModuloBiased(long randomOffset, long numbersInRange)
        {
            var greatestCompleteRange = numbersInRange * (long.MaxValue / numbersInRange);
            return randomOffset > greatestCompleteRange;
        }

        static long PositiveModuloOrZero(long dividend, long divisor)
        {
            Math.DivRem(dividend, divisor, out var mod);
            if (mod < 0)
            {
                mod += divisor;
            }

            return mod;
        }

        static void EnsureMinLEQMax(ref long min, ref long max)
        {
            if (min <= max)
            {
                return;
            }

            var temp = min;
            min = max;
            max = temp;
        }
    }

    /// <summary>
    /// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
    /// Provides a method for performing a deep copy of an object.
    /// Binary Serialization is used to perform the copy.
    /// </summary>
    public static class ObjectCopier
    {
        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            // Don't serialize a null object, simply return the default for that object
            if (source == null)
            {
                return default;
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
