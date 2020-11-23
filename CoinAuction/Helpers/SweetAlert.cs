using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPMAPortal.Client.Helpers
{
  public interface ISweetAlert
  {
    public void Alert(string title, string message, SweetAlert.AlertIcon icon);
  }

  /// <summary>
  /// Service
  /// </summary>
  public class SweetAlert:ISweetAlert
  {
    private readonly IJSRuntime _js;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="js"></param>
    public SweetAlert(IJSRuntime js)
    {
      _js = js;
    }

   /// <summary>
   /// Alert Popup Icons
   /// </summary>
    public enum AlertIcon
    {
      /// <summary>
      /// Question Icon
      /// </summary>
      question,
      /// <summary>
      /// Warning Icon
      /// </summary>
      warning,
      /// <summary>
      /// Error Icon
      /// </summary>
      error,
      /// <summary>
      /// Success Icon
      /// </summary>
      success,
      /// <summary>
      /// Info Icon
      /// </summary>
      info
    }

    /// <summary>
    /// SweetAlert
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="icon"></param>
    public async void Alert(string title, string message, AlertIcon icon)
    {
      await _js.InvokeAsync<object>("Swal.fire", title, message, icon.ToString());
    }
  }
}
