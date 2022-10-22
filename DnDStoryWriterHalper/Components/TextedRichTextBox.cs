using CefSharp.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;
using RichTextBox = Xceed.Wpf.Toolkit.RichTextBox;

namespace DnDStoryWriterHalper.Components
{
    public class TextedRichTextBox : RichTextBox
    {
        public static readonly DependencyProperty FormatedTextProperty = DependencyProperty.Register(
            "FormatedText", typeof(string), typeof(TextedRichTextBox), new PropertyMetadata(default(string)));

        public string FormatedText
        {
            get { return (string) GetValue(FormatedTextProperty); }
            set
            {
                if (_IsNeedsToUpdateProperty)
                {
                    _IsNeedsToUpdateTextChangedEvent = false;
                    using (var s = new MemoryStream())
                    {
                        s.Write(Encoding.ASCII.GetBytes(value));
                        s.Seek(0, SeekOrigin.Begin);
                        TextRange range = new TextRange(Document.ContentStart, Document.ContentEnd);
                        try
                        {
                            range.Load(s, DataFormats.Rtf);
                        }
                        catch (Exception)
                        {
                            Clear();
                        }
                    }

                    _IsNeedsToUpdateTextChangedEvent = true;
                }

                SetValue(FormatedTextProperty, value);
            }
        }
        public event EventHandler FormatedTextChanged;

        private int _UpdateTextFrom;
        private int _UpdatinHandlers;
        private bool _IsNeedsToUpdateTextChangedEvent = true;
        private bool _IsNeedsToUpdateProperty = true;
        public TextedRichTextBox()
        {
            _UpdateTextFrom = 0;
            _UpdatinHandlers = 0;
            TextChanged += async (sender, args) =>
            {
                if (!_IsNeedsToUpdateTextChangedEvent)
                    return;

                _UpdateTextFrom = 10;
                if (_UpdatinHandlers > 1)
                    return;
                _UpdatinHandlers++;
                while (_UpdateTextFrom > 0)
                {
                    await Task.Delay(100);
                    _UpdateTextFrom--;
                    if (_UpdateTextFrom == 0)
                    {
                        string text;
                        using (var ms = new MemoryStream())
                        {
                            var range = new TextRange(Document.ContentStart, Document.ContentEnd);
                            range.Save(ms, DataFormats.Rtf);
                            ms.Seek(0, SeekOrigin.Begin);
                            text = Encoding.ASCII.GetString(ms.GetBuffer(), 0, (int) ms.Length);
                        }
                        
                        _IsNeedsToUpdateProperty = false;
                        FormatedTextChanged?.Invoke(this, EventArgs.Empty);
                        FormatedText = text;
                        _IsNeedsToUpdateProperty = true;
                        _UpdatinHandlers--;
                    }
                }
            };
        }

    }
}
