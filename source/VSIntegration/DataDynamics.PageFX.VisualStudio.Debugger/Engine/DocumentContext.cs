using System;
using Microsoft.VisualStudio.Debugger.Interop;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    // This class represents a document context to the debugger. A document context represents a location within a source file. 
    class DocumentContext : IDebugDocumentContext2
    {
        readonly string m_fileName;
        TEXT_POSITION m_begPos;
        TEXT_POSITION m_endPos;
        readonly CodeContext m_codeContext;


        public DocumentContext(string fileName, TEXT_POSITION begPos, TEXT_POSITION endPos, CodeContext codeContext)
        {
            m_fileName = fileName;
            m_begPos = begPos;
            m_endPos = endPos;
            m_codeContext = codeContext;
        }

        #region IDebugDocumentContext2 Members

        // Compares this document context to a given array of document contexts.
        int IDebugDocumentContext2.Compare(uint Compare, IDebugDocumentContext2[] rgpDocContextSet, uint dwDocContextSetLen, out uint pdwDocContext)
        {
            dwDocContextSetLen = 0;
            pdwDocContext = 0;
            return Const.E_NOTIMPL;
        }

        // Retrieves a list of all code contexts associated with this document context.
        // The engine sample only supports one code context per document context and 
        // the code contexts are always memory addresses.
        int IDebugDocumentContext2.EnumCodeContexts(out IEnumDebugCodeContexts2 ppEnumCodeCxts)
        {
            ppEnumCodeCxts = null;
            try
            {
                ppEnumCodeCxts = new CodeContextEnum(new IDebugCodeContext2[] {m_codeContext});
                return Const.S_OK;
            }
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }
        }

        // Gets the document that contains this document context.
        // This method is for those debug engines that supply documents directly to the IDE. Since the sample engine
        // does not do this, this method returns E_NOTIMPL.
        int IDebugDocumentContext2.GetDocument(out IDebugDocument2 ppDocument)
        {           
            ppDocument = null;
            return Const.E_FAIL;
        }

        // Gets the language associated with this document context.
        int IDebugDocumentContext2.GetLanguageInfo(ref string pbstrLanguage, ref Guid pguidLanguage)
        {
            pbstrLanguage = Const.LangCharp;
            pguidLanguage = Guids.LanguageCSharp;
            return Const.S_OK;
        }

        // Gets the displayable name of the document that contains this document context.
        int IDebugDocumentContext2.GetName(uint gnType, out string pbstrFileName)
        {
            pbstrFileName = m_fileName;
            return Const.S_OK;
        }

        // Gets the source code range of this document context.
        // A source range is the entire range of source code, from the current statement back to just after the previous s
        // statement that contributed code. The source range is typically used for mixing source statements, including 
        // comments, with code in the disassembly window.
        // Sincethis engine does not support the disassembly window, this is not implemented.
        int IDebugDocumentContext2.GetSourceRange(TEXT_POSITION[] pBegPosition, TEXT_POSITION[] pEndPosition)
        {
            throw new NotImplementedException("This method is not implemented");
        }

        // Gets the file statement range of the document context.
        // A statement range is the range of the lines that contributed the code to which this document context refers.
        int IDebugDocumentContext2.GetStatementRange(TEXT_POSITION[] pBegPosition, TEXT_POSITION[] pEndPosition)
        {
            try
            {
                pBegPosition[0].dwColumn = m_begPos.dwColumn;
                pBegPosition[0].dwLine = m_begPos.dwLine;

                pEndPosition[0].dwColumn = m_endPos.dwColumn;
                pEndPosition[0].dwLine = m_endPos.dwLine;
            }
            catch (Exception e)
            {
                return Util.UnexpectedException(e);
            }

            return Const.S_OK;
        }

        // Moves the document context by a given number of statements or lines.
        // This is used primarily to support the Autos window in discovering the proximity statements around 
        // this document context. 
        int IDebugDocumentContext2.Seek(int nCount, out IDebugDocumentContext2 ppDocContext)
        {
            ppDocContext = null;
            return Const.E_NOTIMPL;
        }

        #endregion
    }
}
